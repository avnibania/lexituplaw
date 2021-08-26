using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alifery.Aligo.CandidateScoring.Adapters;
using Alifery.Aligo.CandidateScoring.Models;
using Alifery.Connect.CoreEntities.AligoEntities.Candidate;
using Autofac;

namespace Alifery.Aligo.CandidateScoring
{
    public static class Core
    {
        public static async Task<List<CandidateScoreDto>> EvaluateCandidateScore(
            List<ScoreCriteriaGroupDto> scoreGroups,
            CandidateDto candidate, ILifetimeScope lifetimeScope)
        {
            try
            {
                var candidateScoreDto = new List<CandidateScoreDto>();
                foreach (var group in scoreGroups)
                foreach (var criteria in group.ScoreCriterias)
                {
                    var candidateScore = await EvaluateCriteriaScore(criteria, candidate, lifetimeScope);
                    if (null != candidateScore)
                        candidateScoreDto.Add(candidateScore);
                }
                return candidateScoreDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static async Task<CandidateScoreDto> EvaluateCandidateCriteriaScore(ScoreCriteriaDto criteria,
            CandidateDto candidate, ILifetimeScope lifetimeScope)
        {
            try
            {
                return await EvaluateCriteriaScore(criteria, candidate, lifetimeScope);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private static async Task<CandidateScoreDto> EvaluateCriteriaScore(ScoreCriteriaDto criteria,
            CandidateDto candidate,
            ILifetimeScope lifetimeScope)
        {
            CandidateScoreDto candidateScoreDto = null;

            var evaluator =
                criteria.Evaluators.FirstOrDefault(p => p.ForCandidateType == candidate.CandidateType);
            if (null != evaluator && evaluator.AdapterName > 0)
            {
                var adapterInstance =
                    lifetimeScope.ResolveNamed<ICriteriaEvaluatorAdapter>(evaluator.AdapterName.ToString());
                adapterInstance.Candidate = candidate;
                adapterInstance.Options = criteria.CriteriaOptions;
                var value = await adapterInstance.GetValue();
                if (criteria.CriteriaOptions.Any(p => p.EvaluationValue == value.ToString()))
                {
                    var option =
                        criteria.CriteriaOptions.FirstOrDefault(p => p.EvaluationValue == value.ToString());
                    candidateScoreDto = new CandidateScoreDto
                    {
                        CandidateId = candidate.Id,
                        CriteriaOptionId = option?.Id ?? 0,
                        ScoreCriteria = criteria
                    };
                }
            }

            return candidateScoreDto;
        }
    }
}