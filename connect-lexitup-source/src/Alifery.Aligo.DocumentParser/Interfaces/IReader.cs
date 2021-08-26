using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;
using Alifery.Connect.CoreEntities.AligoEntities.Shared;
using System.IO;
using System.Threading.Tasks;

namespace Alifery.Aligo.DocumentParser.Interfaces
{
    public interface IReader
    {
        Task<string> ReadContent(MemoryStream document, DocumentType type);
        Task<OperationResult<T>> ReadDocument<T>(MemoryStream document, DocumentType type, DocumentCategory category) where T : new();
    }
}
