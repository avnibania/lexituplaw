using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;

namespace Alifery.Aligo.DocumentParser.Models
{
    public class CandidateInsuranceDocumentDataDto
    {
        public int Id { get; set; }
        public int FreelancerInsuranceId { get; set; }
        public int CandidateInsuranceId { get; set; }
        public string InsuranceName { get; set; }
        public string PolicyName { get; set; }
        public string PolicyNumber { get; set; }
        public string LimitOfIndemnity { get; set; }
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public string LawPracticeName { get; set; }
        public string TradingName { get; set; }
        public string IssuerName { get; set; }
        public bool IsActive { get; set; }
        public bool IsCurrent { get; set; }
        public ScanningStatus DocumentScanningStatus { get; set; }
        public string Errors { get; set; }
    }
}