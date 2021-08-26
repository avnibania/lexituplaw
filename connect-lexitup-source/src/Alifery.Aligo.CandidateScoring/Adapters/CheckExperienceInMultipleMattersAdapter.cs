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
    public class CheckExperienceInMultipleMattersAdapter : ICriteriaEvaluatorAdapter
    {
        private readonly IAligoUnitOfWork _auow;

        public CheckExperienceInMultipleMattersAdapter(IAligoUnitOfWork auow)
        {
            _auow = auow;
        }

        public ICollection<ScoreCriteriaOptionsDto> Options { get; set; }
        public CriteriaEvaluatorAdapters AdapterName => CriteriaEvaluatorAdapters.ExperienceInMultipleMatters;

        public CandidateDto Candidate { get; set; }
        public bool CacheOutput { get; set; }

        public async Task<object> GetValue()
        {
            return 0;
        }
    }
}
