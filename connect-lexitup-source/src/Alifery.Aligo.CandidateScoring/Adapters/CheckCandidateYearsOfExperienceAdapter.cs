using Alifery.Aligo.CandidateScoring.Models;
using Alifery.Connect.CoreEntities.AligoEntities.Candidate;
using Alifery.Connect.CoreEntities.AligoEntities.CandidateScoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alifery.Connect.Data;

namespace Alifery.Aligo.CandidateScoring.Adapters
{
    public class CheckCandidateYearsOfExperienceAdapter : ICriteriaEvaluatorAdapter
    {
        private readonly IAligoUnitOfWork _auow;

        public CheckCandidateYearsOfExperienceAdapter(IAligoUnitOfWork auow)
        {
            _auow = auow;
        }


        public CriteriaEvaluatorAdapters AdapterName => CriteriaEvaluatorAdapters.CandidateYearsOfExperience;
        public ICollection<ScoreCriteriaOptionsDto> Options { get; set; }
        public CandidateDto Candidate { get; set; }
        public bool CacheOutput { get; set; }

        public async Task<object> GetValue()
        {
            var noOfYearsOfExp = 0;
            try
            {

                var candidateAdmissions =Candidate.Admissions;
                var candidateExperiences =Candidate.WorkHistory;
                if (null != candidateAdmissions || candidateAdmissions.Count == 0 || null != candidateExperiences ||
                    candidateExperiences.Count == 0)
                    return 0;
                var admissionMinYear = candidateAdmissions.Min(p => p.JurisdictionYear);
                var addmissions = candidateAdmissions.Where(p => p.JurisdictionYear == admissionMinYear);
                var addmissionMinMonth = addmissions.Min(p => p.JurisdictionMonth);
                var dateToCompare = new DateTime(admissionMinYear, addmissionMinMonth, 1);
                var countableExperience = candidateExperiences.Where(p => p.FromDate.Date > dateToCompare.Date);
                if (null != countableExperience)
                    foreach (var workHistory in countableExperience)
                        noOfYearsOfExp += workHistory.FromDate.GetMonthDifference(workHistory.ToDate);
                foreach (var option in Options)
                    if (option.EvaluationValue != null && option.EvaluationValue.Split("-") != null)
                    {
                        var years = option.EvaluationValue.Split("-", StringSplitOptions.RemoveEmptyEntries);
                        if (Convert.ToInt16(years[0]) <= noOfYearsOfExp && Convert.ToInt16(years[1]) > noOfYearsOfExp)
                            return option.EvaluationValue;
                    }
                    else
                    {
                        if (Convert.ToInt16(option.EvaluationValue) == noOfYearsOfExp)
                            return option.EvaluationValue;
                    }
            }
            catch (Exception e)
            {
                return 0;
            }

            return noOfYearsOfExp;
        }
    }
}
