using Alifery.Connect.CoreEntities.AligoEntities.CandidateScoring;
using System.Collections.Generic;

namespace Alifery.Aligo.CandidateScoring.Models
{
    public class ScoreCriteriaDto
    {
        public int Id { get; set; }
        public int CriteriaGroupId { get; set; }
        public string Criteria { get; set; }
        public CriteriaMatchingOption MatchingOption { get; set; }
        public ApprovalCriteria ApprovalCriteria { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ScoreCriteriaOptionsDto> CriteriaOptions { get; set; }
        public ICollection<CriteriaEvaluatorDto> Evaluators { get; set; }
    }
}
