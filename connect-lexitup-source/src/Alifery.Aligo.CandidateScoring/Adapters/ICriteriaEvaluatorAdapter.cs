using Alifery.Aligo.CandidateScoring.Models;
using Alifery.Connect.CoreEntities.AligoEntities.Candidate;
using Alifery.Connect.CoreEntities.AligoEntities.CandidateScoring;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alifery.Aligo.CandidateScoring.Adapters
{
    public interface ICriteriaEvaluatorAdapter
    {
        CriteriaEvaluatorAdapters AdapterName { get; }
        CandidateDto Candidate { get; set; }
        ICollection<ScoreCriteriaOptionsDto> Options { get; set; }
        bool CacheOutput { get; set; }
        Task<object> GetValue();
    }
}
