using Alifery.Aligo.CandidateScoring.Models;
using Alifery.Connect.CoreEntities.AligoEntities.Candidate;
using Alifery.Connect.CoreEntities.AligoEntities.CandidateScoring;
using Alifery.Connect.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alifery.Aligo.CandidateScoring.Adapters
{
    public class CheckPedigreeOfThePersonWorkingForAdapter : ICriteriaEvaluatorAdapter
    {
        private readonly IAligoUnitOfWork _auow;

        public CheckPedigreeOfThePersonWorkingForAdapter(IAligoUnitOfWork auow)
        {
            _auow = auow;
        }

        public CriteriaEvaluatorAdapters AdapterName => CriteriaEvaluatorAdapters.PedigreeOfThePersonWorkingFor;

        public CandidateDto Candidate { get; set; }
        public ICollection<ScoreCriteriaOptionsDto> Options { get; set; }
        public bool CacheOutput { get; set; }

        public async Task<object> GetValue()
        {
            return 0;
        }
    }
}
