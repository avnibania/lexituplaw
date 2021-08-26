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
    public class CheckFirmHasCandidateNameAdapter : ICriteriaEvaluatorAdapter
    {
        private readonly IAligoUnitOfWork _auow;

        public CheckFirmHasCandidateNameAdapter(IAligoUnitOfWork auow)
        {
            _auow = auow;
        }

        public CriteriaEvaluatorAdapters AdapterName => CriteriaEvaluatorAdapters.FirmHasCandidateName;

        public CandidateDto Candidate { get; set; }
        public ICollection<ScoreCriteriaOptionsDto> Options { get; set; }
        public bool CacheOutput { get; set; }

        public async Task<object> GetValue()
        {
            try
            {
                var candidateWorkHistory = Candidate.WorkHistory;
                if (null == candidateWorkHistory)
                {
                    return 0;
                }

                if (candidateWorkHistory?.Any(p =>
                        p.FirmName.Contains(Candidate.FirstName) || p.FirmName.Contains(Candidate.LastName)) ?? false)
                {
                    return 1;
                }

                return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
