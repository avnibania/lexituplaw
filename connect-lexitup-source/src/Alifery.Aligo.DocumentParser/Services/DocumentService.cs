using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Alifery.Aligo.DocumentParser.External.AWS;
using Alifery.Aligo.DocumentParser.Interfaces;
using Alifery.Aligo.DocumentParser.Models;
using Alifery.Connect.CoreEntities.AligoEntities.Candidate;
using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;
using Alifery.Connect.CoreEntities.AligoEntities.Search;
using Alifery.Connect.CoreEntities.AligoEntities.Shared;
using Alifery.Connect.Data;
using Alifery.Connect.Data.AligoEntity.DocumentParser;
using Alifery.Connect.Data.AligoEntity.Search;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Alifery.Aligo.DocumentParser.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IAwsService _amazonService;
        private readonly IAligoUnitOfWork _auow;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IReader _reader;

        public DocumentService(IReader reader, IMapper mapper, IAwsService amazonService, ILoggerFactory loggerFactory,
            IConfiguration configuration, IAligoUnitOfWork auow)
        {
            _reader = reader;
            _mapper = mapper;
            _amazonService = amazonService;
            _configuration = configuration;
            _auow = auow;
            _logger = loggerFactory.CreateLogger<DocumentService>();
        }

        public async Task<OperationResult<CandidatePracticingCertificateDocumentDataDto>>
            ParseFreelancerCertificatesAsync(
                string filePath, bool retryError = false)
        {
            var type = Path.GetExtension(filePath.ToLower()) ==
                       $".{DocumentType.Pdf.ToString().ToLower()}"
                ? DocumentType.Pdf
                : DocumentType.Doc;
            var file = await _amazonService.Download(filePath);
            var result =
                await _reader.ReadDocument<CandidatePracticingCertificateDocumentDataDto>(file, type,
                    DocumentCategory.FreelancerCertificate);
            return result;
        }

        public async Task<OperationResult<CandidateInsuranceDocumentDataDto>> ParseFreelancerInsurancesAsync(
            string filePath, bool retryError = false)
        {
            var type = Path.GetExtension(filePath.ToLower()) ==
                       $".{DocumentType.Pdf.ToString().ToLower()}"
                ? DocumentType.Pdf
                : DocumentType.Doc;
            var file = await _amazonService.Download(filePath);
            var result =
                await _reader.ReadDocument<CandidateInsuranceDocumentDataDto>(file, type,
                    DocumentCategory.FreelancerInsurance);
            return result;
        }

        public async Task<CandidatePracticingCertificateDocumentDataDto> ParseCertificateDocumentAsync(
            string filePath, bool retryError = false)
        {
            var type = Path.GetExtension(filePath.ToLower()) ==
                       $".{DocumentType.Pdf.ToString().ToLower()}"
                ? DocumentType.Pdf
                : DocumentType.Doc;
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var file = new MemoryStream();
            fileStream.CopyTo(file); // await _amazonService.Download(filePath);
            var result =
                await _reader.ReadDocument<CandidatePracticingCertificateDocumentDataDto>(file, type,
                    DocumentCategory.FreelancerCertificate);
            return result.Result;
        }

        public async Task<CandidateInsuranceDocumentDataDto> ParseInsuranceDocumentAsync(
            string filePath, bool retryError = false)
        {
            var type = Path.GetExtension(filePath.ToLower()) ==
                       $".{DocumentType.Pdf.ToString().ToLower()}"
                ? DocumentType.Pdf
                : DocumentType.Doc;
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var file = new MemoryStream();
            fileStream.CopyTo(file); // await _amazonService.Download(filePath);
            var result =
                await _reader.ReadDocument<CandidateInsuranceDocumentDataDto>(file, type,
                    DocumentCategory.FreelancerInsurance);
            return result.Result;
        }

        public async Task<CandidatePracticingCertificateDocumentDataDto> SaveFreelancerPracticingCertificateBackendData(
            CandidatePracticingCertificateDocumentDataDto dto)
        {
            var existing = await _auow.CandidatePracticingCertificateDocumentData.All()
                .Where(p => p.FreelancerCertificateId == dto.FreelancerCertificateId).FirstOrDefaultAsync();
            if (null == existing)
            {
                var newCertificate = _mapper.Map<CandidatePracticingCertificateDocumentData>(dto);
                await _auow.CandidatePracticingCertificateDocumentData.AddAsync(newCertificate);
                await _auow.SaveChangesAsync();
                return _mapper.Map<CandidatePracticingCertificateDocumentDataDto>(newCertificate);
            }

            existing.APCN = dto.APCN;
            existing.FreelancerNameOnDocument = dto.FreelancerNameOnDocument;
            existing.IsActive = dto.IsActive;
            existing.IsCurrent = dto.IsCurrent;
            existing.State = dto.State;
            existing.ValidFrom = dto.ValidFrom;
            existing.ValidTo = dto.ValidTo;
            existing.Errors = dto.Errors;
            await _auow.SaveChangesAsync();
            return _mapper.Map<CandidatePracticingCertificateDocumentDataDto>(existing);
        }

        public async Task<CandidateInsuranceDocumentDataDto> SaveFreelancerInsuranceBackendData(
            CandidateInsuranceDocumentDataDto dto)
        {
            var existing = await _auow.CandidateInsuranceDocumentData.All()
                .Where(p => p.FreelancerInsuranceId == dto.FreelancerInsuranceId).FirstOrDefaultAsync();
            if (null == existing)
            {
                var newInsurance = _mapper.Map<CandidateInsuranceDocumentData>(dto);
                await _auow.CandidateInsuranceDocumentData.AddAsync(newInsurance);
                await _auow.SaveChangesAsync();
                return _mapper.Map<CandidateInsuranceDocumentDataDto>(newInsurance);
            }

            existing.InsuranceName = dto.InsuranceName;
            existing.LimitOfIndemnity = dto.LimitOfIndemnity;
            existing.IsActive = dto.IsActive;
            existing.IsCurrent = dto.IsCurrent;
            existing.IssuerName = dto.IssuerName;
            existing.PeriodFrom = dto.PeriodFrom;
            existing.PeriodTo = dto.PeriodTo;
            existing.LawPracticeName = dto.LawPracticeName;
            existing.PolicyNumber = dto.PolicyNumber;
            existing.TradingName = dto.TradingName;
            existing.Errors = dto.Errors;
            await _auow.SaveChangesAsync();
            return _mapper.Map<CandidateInsuranceDocumentDataDto>(existing);
        }

        public async Task SendFreelancerCertificateData(CandidatePracticingCertificateDocumentDataDto data)
        {
            using (var client = new HttpClient())
            {
                var url = _configuration.GetSection("AliferyAPIs:SendFreelancerCertificateData").Value;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(
                            string.Format("{0}:{1}", "admin", "H#ll0Al1feryAl1g0S#cur1tyK#y"))));

                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                var httpContent = new StringContent(json);
                var response = await client.PostAsync(url, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    //Save Unsuccessful records
                }

            }
        }

        public async Task RefreshCandidateContent()
        {
          
        }

        public async Task<string> ExtractFileContent(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;

            var type = Path.GetExtension(filePath.ToLower()) ==
                       $".{DocumentType.Pdf.ToString().ToLower()}"
                ? DocumentType.Pdf
                : DocumentType.Doc;
            try
            {
                var file = await _amazonService.Download(filePath);
                var content = await _reader.ReadContent(file, type);
                return content;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<DocumentFormatDto> CreateDocumentFormat(DocumentFormatDto dto)
        {
            var entity = _mapper.Map<DocumentFormat>(dto);
            if (dto?.FormatIdentifiers != null)
            {
                entity.FormatIdentifiers = new List<DocumentFormatIdentifier>();
                foreach (var formatIdentifier in dto.FormatIdentifiers)
                {
                    var identifier = new DocumentFormatIdentifier
                    {
                        DocumentFormatId = formatIdentifier.DocumentFormatId,
                        GroupMethod = formatIdentifier.GroupMethod,
                        GroupKey = formatIdentifier.GroupKey,
                        IdentifierType = formatIdentifier.IdentifierType
                    };
                    if (formatIdentifier.Mappings != null)
                    {
                        identifier.Mappings = new List<DocumentFormatIdertifierMapping>();
                        foreach (var mapping in formatIdentifier.Mappings)
                            identifier.Mappings.Add(
                                new DocumentFormatIdertifierMapping {Map = _mapper.Map<Map>(mapping)});
                    }
                    entity.FormatIdentifiers.Add(identifier);
                }
            }

            entity.Id = 0;
            await _auow.DocumentFormat.AddAsync(entity);
            await _auow.SaveChangesAsync();
            return _mapper.Map<DocumentFormatDto>(entity);
        }

        public async Task<DocumentFormatDto> UpdateDocumentFormat(DocumentFormatDto dto)
        {
            var entity = await _auow.DocumentFormat.GetSingleOrDefaultAsync(p => p.Id == dto.Id);
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.DocumentCategory = dto.DocumentCategory;
            entity.DocumentType = dto.DocumentType;
            entity.IsActive = dto.IsActive;
            await _auow.SaveChangesAsync();
            return _mapper.Map<DocumentFormatDto>(entity);
        }

        public async Task<DocumentFormatDto> GetDocumentFormat(int id, params DocumentFormatSearchOptions[] options)
        {
            var query = _auow.DocumentFormat.All().Where(p => p.Id == id);
            query = ApplyDocumentFormatIncludeConditions(options, query);
            var entity = await query.FirstOrDefaultAsync();
            return _mapper.Map<DocumentFormatDto>(entity);
        }

        public async Task<bool> ClearDocumentFormats()
        {
            var formats = await _auow.DocumentFormat.GetAllAsync();
            var maps = await _auow.Map.GetAllAsync();
            _auow.Map.RemoveRange(maps);
            _auow.DocumentFormat.RemoveRange(formats);
            await _auow.SaveChangesAsync();
            return true;
        }

        private async Task SaveContent(string filePath, CandidateDto candidate, ContentSource source)
        {
            if (string.IsNullOrEmpty(filePath) || null == candidate)
                return;

            var type = Path.GetExtension(filePath.ToLower()) ==
                       $".{DocumentType.Pdf.ToString().ToLower()}"
                ? DocumentType.Pdf
                : DocumentType.Doc;
            try
            {
                var file = await _amazonService.Download(filePath);
                var content = await _reader.ReadContent(file, type);
                await SaveContent(candidate, source, content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task ClearContent(CandidateDto candidate)
        {
            var existing = await _auow.CandidateContent.All()
                .Where(p => p.CandidateId == candidate.Id).ToListAsync();
            foreach (var existingContent in existing)
                _auow.CandidateContent.Remove(existingContent);

            await _auow.SaveChangesAsync();
        }

        private async Task SaveContent(CandidateDto candidate, ContentSource source, string content)
        {
           
            var candidateContent = new CandidateContent
            {
                CandidateId = candidate.Id,
                Content = content,
                ContentSource = source
            };
            await _auow.CandidateContent.AddAsync(candidateContent);
            await _auow.SaveChangesAsync();
        }

        private static IQueryable<DocumentFormat> ApplyDocumentFormatIncludeConditions(
            DocumentFormatSearchOptions[] options,
            IQueryable<DocumentFormat> query)
        {
            if (options.Contains(DocumentFormatSearchOptions.IncludeAll))
                if (options.Contains(DocumentFormatSearchOptions.IncludeAll) ||
                    options.Contains(DocumentFormatSearchOptions.IncludeFields))
                    query = query.Include(p => p.Fields).ThenInclude(p => p.Mappings);

            if (options.Contains(DocumentFormatSearchOptions.IncludeAll) ||
                options.Contains(DocumentFormatSearchOptions.IncludeIdentifiers))
                query = query.Include(p => p.FormatIdentifiers).ThenInclude(p => p.Mappings);

            return query;
        }
    }
}