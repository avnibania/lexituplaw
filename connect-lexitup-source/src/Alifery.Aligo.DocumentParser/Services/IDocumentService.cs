using System.Threading.Tasks;
using Alifery.Aligo.DocumentParser.Models;
using Alifery.Connect.CoreEntities.AligoEntities.Shared;

namespace Alifery.Aligo.DocumentParser.Services
{
    public interface IDocumentService
    {
        Task<DocumentFormatDto> CreateDocumentFormat(DocumentFormatDto dto);
        Task<DocumentFormatDto> UpdateDocumentFormat(DocumentFormatDto dto);
        Task<DocumentFormatDto> GetDocumentFormat(int id, params DocumentFormatSearchOptions[] options);

        Task<OperationResult<CandidatePracticingCertificateDocumentDataDto>> ParseFreelancerCertificatesAsync(
            string filePath, bool retryError = false);

        Task<OperationResult<CandidateInsuranceDocumentDataDto>> ParseFreelancerInsurancesAsync(
            string filePath, bool retryError = false);

        Task<CandidatePracticingCertificateDocumentDataDto> ParseCertificateDocumentAsync(
            string filePath, bool retryError = false);

        Task<CandidateInsuranceDocumentDataDto> ParseInsuranceDocumentAsync(
            string filePath, bool retryError = false);

        Task RefreshCandidateContent();
        Task SendFreelancerCertificateData(CandidatePracticingCertificateDocumentDataDto data);

        Task<CandidatePracticingCertificateDocumentDataDto> SaveFreelancerPracticingCertificateBackendData(
            CandidatePracticingCertificateDocumentDataDto dto);

        Task<CandidateInsuranceDocumentDataDto> SaveFreelancerInsuranceBackendData(
            CandidateInsuranceDocumentDataDto dto);

        Task<string> ExtractFileContent(string filePath);
        Task<bool> ClearDocumentFormats();
    }
}