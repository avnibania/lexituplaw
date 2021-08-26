using System;
using System.IO;
using System.Threading.Tasks;

namespace Alifery.Aligo.DocumentParser.External.AWS
{
    public interface IAwsService
    {
        Task<Tuple<bool, string>> Upload(string filePath, MemoryStream ms, bool isAsync, DateTime? expires = null, bool enablePublicRead = false);

        Task<MemoryStream> Download(string filepath);

    }
}
