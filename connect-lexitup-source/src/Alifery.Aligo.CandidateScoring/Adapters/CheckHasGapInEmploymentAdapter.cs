using Alifery.Aligo.CandidateScoring.Models;
using Alifery.Connect.CoreEntities.AligoEntities.Candidate;
using Alifery.Connect.CoreEntities.AligoEntities.CandidateScoring;
using Alifery.Connect.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alifery.Aligo.CandidateScoring.Adapters
{
    public class CheckHasGapInEmploymentAdapter : ICriteriaEvaluatorAdapter
    {
        private readonly IAligoUnitOfWork _auow;

        public CheckHasGapInEmploymentAdapter(IAligoUnitOfWork auow)
        {
            _auow = auow;
        }

        public CriteriaEvaluatorAdapters AdapterName => CriteriaEvaluatorAdapters.HasGapInEmployment;

        public CandidateDto Candidate { get; set; }
        public ICollection<ScoreCriteriaOptionsDto> Options { get; set; }
        public bool CacheOutput { get; set; }

        public async Task<object> GetValue()
        {
            try
            {
                var candidateWorks = Candidate.WorkHistory?.ToList();
                if (null == candidateWorks || candidateWorks.Count == 0)
                {
                    return 0;
                }

                var previousMonth = 0;
                for (var i = 1; i < candidateWorks.Count; i++)
                {
                    if (candidateWorks[i - 1].ToDate.GetMonthDifference(candidateWorks[i].FromDate) > 2)
                    {
                        return 1;
                    }
                }

                return 2;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
