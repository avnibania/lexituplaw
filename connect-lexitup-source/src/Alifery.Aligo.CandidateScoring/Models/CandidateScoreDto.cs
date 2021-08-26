namespace Alifery.Aligo.CandidateScoring.Models
{
    public class CandidateScoreDto
    {
        public int CandidateId { get; set; }
        public ScoreCriteriaDto ScoreCriteria { get; set; }
        public int CriteriaOptionId { get; set; }
        public bool BinaryValueOption { get; set; }
        public string StringValueOption { get; set; }
        public int NumberValueOption { get; set; }
        public decimal DecimalValueOption { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }
        public bool IsLockedForRefresh { get; set; }
    }
}
