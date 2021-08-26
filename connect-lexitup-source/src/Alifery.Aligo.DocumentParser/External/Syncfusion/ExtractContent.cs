using Alifery.Aligo.DocumentParser.Models;
using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Alifery.Aligo.DocumentParser.External.Syncfusion
{
    public class ExtractContent : IExtractContent
    {
        private readonly ILogger<ExtractContent> _logger;

        public async Task<DocumentContentModel> Extract(DocumentType type, Stream fileStreamInput)
        {
            switch (type)
            {
                case DocumentType.Doc:
                    var contentExtract = new ExtractWord();
                    return await contentExtract.ExtractContent(fileStreamInput);
                case DocumentType.Pdf:
                    var contentExtractPdf = new ExtractPdf();
                    return await contentExtractPdf.ExtractContent(fileStreamInput);
            }
            return new DocumentContentModel();
        }

        public async Task<string> ExtractPlainText(DocumentType type, Stream fileStreamInput)
        {
            switch (type)
            {
                case DocumentType.Doc:
                    var contentExtract = new ExtractWord();
                    return await contentExtract.ExtractPlainText(fileStreamInput);
                case DocumentType.Pdf:
                    var contentExtractPdf = new ExtractPdf();
                    return await contentExtractPdf.ExtractPlainText(fileStreamInput);
            }
            return string.Empty;
        }
    }
}
