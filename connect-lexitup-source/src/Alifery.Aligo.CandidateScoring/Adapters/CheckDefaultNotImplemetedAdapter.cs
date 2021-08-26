using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alifery.Aligo.CandidateScoring.Models;
using Alifery.Connect.CoreEntities.AligoEntities.Candidate;
using Alifery.Connect.CoreEntities.AligoEntities.CandidateScoring;
using Alifery.Connect.Data;

namespace Alifery.Aligo.CandidateScoring.Adapters
{
    public class CheckDefaultNotImplemetedAdapter : ICriteriaEvaluatorAdapter
    {
        private readonly IAligoUnitOfWork _auow;

        public CheckDefaultNotImplemetedAdapter(IAligoUnitOfWork auow)
        {
            _auow = auow;
        }

        public CriteriaEvaluatorAdapters AdapterName => CriteriaEvaluatorAdapters.DefaultNotImplemeted;
        public ICollection<ScoreCriteriaOptionsDto> Options { get; set; }
        public CandidateDto Candidate { get; set; }
        public bool CacheOutput { get; set; }

        public async Task<object> GetValue()
        {
            return 0;
        }
    }
}
