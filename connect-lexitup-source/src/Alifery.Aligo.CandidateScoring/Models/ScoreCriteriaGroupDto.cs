using Alifery.Connect.CoreEntities.AligoEntities.CandidateScoring;
using System.Collections.Generic;

namespace Alifery.Aligo.CandidateScoring.Models
{
    public class ScoreCriteriaGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StepNo { get; set; }
        public string Description { get; set; }
        public ApprovalCriteria ApprovalCriteria { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ScoreCriteriaDto> ScoreCriterias { get; set; }
    }
}
