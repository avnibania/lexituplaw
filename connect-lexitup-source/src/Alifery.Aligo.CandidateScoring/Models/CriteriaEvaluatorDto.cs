using Alifery.Connect.CoreEntities.AligoEntities.CandidateScoring;
using Alifery.Connect.CoreEntities.AligoEntities.Shared;

namespace Alifery.Aligo.CandidateScoring.Models
{
    public class CriteriaEvaluatorDto
    {
        public ScoreCriteriaDto ScoreCriteria { get; set; }
        public int ScoreCriteriaId { get; set; }
        public bool IsValid { get; set; }
        public CandidateType ForCandidateType { get; set; }
        public CriteriaEvaluatorAdapters AdapterName { get; set; }
    }
}
