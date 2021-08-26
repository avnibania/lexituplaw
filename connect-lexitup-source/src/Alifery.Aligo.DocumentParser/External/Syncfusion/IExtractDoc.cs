using Alifery.Aligo.DocumentParser.Models;
using System.IO;
using System.Threading.Tasks;

namespace Alifery.Aligo.DocumentParser.External.Syncfusion
{
    public interface IExtractDoc
    {
        Task<DocumentContentModel> ExtractContent(Stream fileStreamInput);
        Task<string> ExtractPlainText(Stream fileStreamInput);

    }
}
