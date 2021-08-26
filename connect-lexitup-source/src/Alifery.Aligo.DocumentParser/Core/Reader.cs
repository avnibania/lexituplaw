using Alifery.Aligo.DocumentParser.External.Syncfusion;
using Alifery.Aligo.DocumentParser.Interfaces;
using Alifery.Aligo.DocumentParser.Models;
using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;
using Alifery.Connect.CoreEntities.AligoEntities.Shared;
using Alifery.Connect.Data;
using Alifery.Connect.Data.AligoEntity.DocumentParser;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Alifery.Aligo.DocumentParser.Core
{
    public class Reader : IReader
    {
        private readonly IExtractContent _extractContent;
        private readonly IMapper _mapper;
        private readonly IAligoUnitOfWork _auow;

        public Reader(IExtractContent extractContent, IMapper mapper, IAligoUnitOfWork auow)
        {
            _extractContent = extractContent;
            _mapper = mapper;
            _auow = auow;
        }

        public async Task<string> ReadContent(MemoryStream document, DocumentType type)
        {
            var documentContent = await _extractContent.ExtractPlainText(type, document);
            return documentContent;
        }

        public async Task<OperationResult<T>> ReadDocument<T>(MemoryStream document, DocumentType type, DocumentCategory category) where T : new()
        {
            var newRecord = new T();
            var errorResponse = new OperationResult<T>() { Result = default(T) };
            try
            {

                var documentContent = await _extractContent.Extract(type, document);
                var variables = new List<KeyValuePair<string, string>>();
                var formatData =
                    await _auow.DocumentFormat.All().Include(p => p.FormatIdentifiers).ThenInclude(p => p.Mappings).ThenInclude(p => p.Map).ThenInclude(p => p.Parameters)
                        .Include(p => p.FormatIdentifiers).ThenInclude(p => p.Mappings).ThenInclude(p => p.Map).ThenInclude(p => p.PostProcessMaps).ThenInclude(p => p.Parameters)
                        .Include(p => p.Fields).ThenInclude(p => p.Mappings).ThenInclude(p => p.Map).ThenInclude(p => p.Parameters)
                        .Include(p => p.Fields).ThenInclude(p => p.Mappings).ThenInclude(p => p.Map).ThenInclude(p => p.PostProcessMaps).ThenInclude(p => p.Parameters).Where(p => p.DocumentCategory == category && p.DocumentType == type).ToListAsync();


                if (null == formatData)
                {
                    errorResponse.AddError("No matching format found for this category and document type");
                    return errorResponse;
                }

                var formatDataDto = _mapper.Map<List<DocumentFormatDto>>(formatData);

                var result = Interpreter.Compile(documentContent, category, formatDataDto, variables);
                if (result.Count > 0)
                {

                    foreach (var parseResult in result)
                    {
                        if (parseResult.Success)
                        {
                            PropertyInfo propertyInfo = newRecord.GetType().GetProperty(parseResult.Result.FieldName);
                            propertyInfo.SetValue(newRecord, Convert.ChangeType(parseResult.Result.Value, propertyInfo.PropertyType),
                                null);
                        }
                        else
                        {
                            if (null != parseResult.Errors)
                            {
                                foreach (var error in parseResult.Errors)
                                {

                                    await _auow.DocumentParserErrorLogs.AddAsync(new DocumentParserErrorLogs()
                                    {
                                        CreatedBy = "Document Parser",
                                        MethodName = "ReadDocument",
                                        ErrorText = error,
                                        Description = $"Error Returned from Compile Document for field:{parseResult?.Result?.FieldName}"
                                    });
                                }
                                await _auow.SaveChangesAsync();
                            }


                        }
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _auow.DocumentParserErrorLogs.AddAsync(new DocumentParserErrorLogs()
                {
                    CreatedBy = "Document Parser",
                    MethodName = "ReadDocument",
                    ErrorText = e.Message,
                    Description = "Error Parsing Document"
                });
                await _auow.SaveChangesAsync();
                errorResponse.AddError($"Error in Parsing: {e.Message}");
                return errorResponse;
            }
            return new OperationResult<T>() { Result = newRecord };
        }
    }
}
