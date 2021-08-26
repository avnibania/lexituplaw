using Alifery.Aligo.DocumentParser.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alifery.Aligo.DocumentParser.External.Syncfusion
{
    public class ExtractPdf : IExtractDoc
    {
        public async Task<DocumentContentModel> ExtractContent(Stream fileStreamInput)
        {
            var documentContentModel = new DocumentContentModel();

            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fileStreamInput);

            documentContentModel.DocumentInfo = loadedDocument.DocumentInformation;
            documentContentModel.Pages = new List<DocumentPageModel>();

            foreach (PdfPageBase page in loadedDocument.Pages)
            {
                var pageDoc = new DocumentPageModel();
                var text = page.ExtractText(true);
                string[] lines = text.Split("\r\n");
                pageDoc.ContentLines.AddRange(lines.ToList());
                documentContentModel.Pages.Add(pageDoc);
            }

            return documentContentModel;
        }

        public async Task<string> ExtractPlainText(Stream fileStreamInput)
        {
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fileStreamInput);
            string returnText = string.Empty;
            foreach (PdfPageBase page in loadedDocument.Pages)
            {
                var text = page.ExtractText(true);
                returnText += text;
            }
            return returnText;
        }
    }
}
