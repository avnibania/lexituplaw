using Alifery.Aligo.CandidateScoring.Models;
using Alifery.Connect.CoreEntities.AligoEntities.Candidate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alifery.Aligo.CandidateScoring.Services
{
    public interface ICandidateScoringService
    {
        Task<bool> SaveCandidateScore(CandidateDto candidate, List<CandidateScoreDto> candidateScores, string updatedBy);
        Task<List<CandidateScoreDto>> EvaluateCandidateCriteria(CandidateDto candidate);
        Task<CandidateScoreDto> EvaluateCandidateCriteria(CandidateDto candidate, int criteriaId);
        Task<List<ScoreCriteriaGroupDto>> GetAllGroupCriteria();
        Task<List<CandidateScoreDto>> GetCandidateScores(int candidateId);
        Task<ScoreCriteriaGroupDto> GetGroupCriteria(int criteriaGroupId);
        Task<bool> ResetLock(int candidateId, int criteriaId);

    }
}
