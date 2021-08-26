namespace Alifery.Aligo.CandidateScoring.Models
{
    public class ScoreCriteriaOptionsDto
    {
        public int Id { get; set; }
        public int ScoreCriteriaId { get; set; }
        public bool BinaryValueOption { get; set; }
        public string StringValueOption { get; set; }
        public int NumberValueOption { get; set; }
        public decimal DecimalValueOption { get; set; }
        public decimal Weighting { get; set; }
        public decimal Score { get; set; }
        public bool IsActive { get; set; }
        public string EvaluationValue { get; set; }
        public string MatchingReferenceValue { get; set; }
    }
}
