using Alifery.Aligo.DocumentParser.Models;
using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;
using System.IO;
using System.Threading.Tasks;

namespace Alifery.Aligo.DocumentParser.External.Syncfusion
{
    public interface IExtractContent
    {
        Task<DocumentContentModel> Extract(DocumentType type, Stream fileStreamInput);
        Task<string> ExtractPlainText(DocumentType type, Stream fileStreamInput);
    }
}
