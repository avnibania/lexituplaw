using Alifery.Aligo.DocumentParser.Models;
using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;
using Alifery.Connect.CoreEntities.AligoEntities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alifery.Aligo.DocumentParser.Core
{
    public static class Interpreter
    {
        public static List<OperationResult<InterpretedField>> Compile(DocumentContentModel model, DocumentCategory category,
            List<DocumentFormatDto> formats, List<KeyValuePair<string, string>> variables = null)
        {
            var result = new List<OperationResult<InterpretedField>>();
            model.Errors = new List<string>();
            foreach (var format in formats)
            {
                try
                {
                    if (MapResolver.CheckFormatMatch(format.FormatIdentifiers, model))
                    {
                        foreach (var mapping in format.Fields.Where(p => p.DerivedField == false))
                        {
                            try
                            {
                                var value = MapResolver.ProcessMap(model, mapping.Mappings, variables);
                                result.Add(new OperationResult<InterpretedField>() { Result = new InterpretedField(mapping.FieldName, value) });
                            }
                            catch (Exception e)
                            {
                                var errorResult = new OperationResult<InterpretedField>()
                                {
                                    Result = new InterpretedField(mapping.FieldName, string.Empty)
                                };
                                errorResult.AddError(e.Message);
                                result.Add(errorResult);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    var errorResult = new OperationResult<InterpretedField>();
                    errorResult.AddError(e.Message);
                    result.Add(errorResult);
                }
            }
            return result;
        }
    }
}
