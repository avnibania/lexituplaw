using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon;

namespace Alifery.Aligo.DocumentParser.External.AWS
{
    public class AwsService : IAwsService
    {
        private readonly IAmazonS3 _client;
        private readonly IConfiguration _configuration;
        public static readonly List<string> AllowedFileTypes = new List<string> { ".htm", ".html", ".pdf", ".csv", ".xls",
            ".xlsx", ".jpg", ".jpeg", ".png", ".gif", ".doc", ".docx", ".txt", ".xml"};

        public AwsService(IConfiguration configuration)
        {
            var credentials = new BasicAWSCredentials(configuration.GetSection("AWS:AccessKey").Value,
                configuration.GetSection("AWS:SecretKey").Value);
            _client = new AmazonS3Client(credentials, RegionEndpoint.APSoutheast2);
            _configuration = configuration;
        }

        public async Task<Tuple<bool, string>> Upload(string filePath, MemoryStream ms, bool isAsync, DateTime? expires = null,
            bool enablePublicRead = false)
        {
            if (!AllowedFileTypes.Contains(Path.GetExtension(filePath)))
            {
                return Tuple.Create(false, "File type is not allowed");
            }

            ms.Seek(0, SeekOrigin.Begin);

            var request = new PutObjectRequest
            {
                BucketName = _configuration.GetSection("AWS:BucketName").Value
                ,
                Key = filePath,
                InputStream = ms
            };

            if (enablePublicRead)
            {
                request.CannedACL = S3CannedACL.PublicRead;
            }

            var uploadResult = new PutObjectResponse();
            try
            {
                uploadResult = await _client.PutObjectAsync(request);
            }
            catch (AmazonS3Exception ex)
            {
                return Tuple.Create(false, ex.ToString());
            }
            return Tuple.Create(uploadResult.HttpStatusCode == HttpStatusCode.OK,
                uploadResult.HttpStatusCode.ToString());
        }

        public async Task<MemoryStream> Download(string filepath)
        {
            var request = new GetObjectRequest
            {
                BucketName = _configuration.GetSection("AWS:BucketName").Value,
                Key = filepath
            };

            using (var response = await _client.GetObjectAsync(request))
            {
                var ms = new MemoryStream();
                response.ResponseStream.CopyTo(ms);
                return ms;
            }
        }
    }

    public class InvalidFileTypeException : Exception
    {
        public InvalidFileTypeException() { }
        public InvalidFileTypeException(string message) : base(message) { }
    }
}
