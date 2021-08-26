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
    public class CheckValidCandidateInsuranceAdapter : ICriteriaEvaluatorAdapter
    {
        private readonly IAligoUnitOfWork _auow;

        public CheckValidCandidateInsuranceAdapter(IAligoUnitOfWork auow)
        {
            _auow = auow;
        }

        public CriteriaEvaluatorAdapters AdapterName => CriteriaEvaluatorAdapters.ValidCandidateInsurance;

        public CandidateDto Candidate { get; set; }
        public ICollection<ScoreCriteriaOptionsDto> Options { get; set; }
        public bool CacheOutput { get; set; }

        public async Task<object> GetValue()
        {
            try
            {
                var candidateInsurance = Candidate.Insurances;
                if (null == candidateInsurance || candidateInsurance.Count == 0)
                {
                    return 0;
                }

                if (candidateInsurance?.Any(p =>
                        p.ValidFrom.Date < DateTime.Now.Date && p.ValidTo > DateTime.UtcNow.Date) ?? false)
                {
                    return 1;
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
