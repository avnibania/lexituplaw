using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;

namespace Alifery.Aligo.DocumentParser.Models
{
    public class CandidatePracticingCertificateDocumentDataDto
    {
        public int Id { get; set; }
        public int CandidateCertificateId { get; set; }
        public int FreelancerCertificateId { get; set; }
        public string FreelancerNameOnDocument { get; set; }
        public int PassId { get; set; }
        public string APCN { get; set; }
        public string State { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string IsCurrent { get; set; }
        public string TypeOfCertificate { get; set; }
        public bool IsActive { get; set; }
        public ScanningStatus DocumnScanningStatus { get; set; }
        public string Errors { get; set; }
    }
}