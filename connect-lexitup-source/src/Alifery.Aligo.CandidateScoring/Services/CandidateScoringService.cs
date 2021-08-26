using Alifery.Aligo.CandidateScoring.Models;
using Alifery.Connect.CoreEntities.AligoEntities.Candidate;
using Alifery.Connect.Data;
using Alifery.Connect.Data.AligoEntity.CandidateScoring;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alifery.Aligo.CandidateScoring.Services
{
    public class CandidateScoringService : ICandidateScoringService
    {
        private readonly IAligoUnitOfWork _auow;
        private readonly ILogger<CandidateScoringService> _logger;
        private readonly IMapper _mapper;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IConfiguration _configuration;

        public CandidateScoringService(ILogger<CandidateScoringService> logger, IMapper mapper, ILifetimeScope lifetimeScope, IConfiguration configuration, IAligoUnitOfWork auow)
        {

            _logger = logger;
            _mapper = mapper;
            _lifetimeScope = lifetimeScope;
            _configuration = configuration;
            _auow = auow;
        }


        public async Task<List<ScoreCriteriaGroupDto>> GetAllGroupCriteria()
        {
            var groups = await _auow.ScoreCriteriaGroup.All().Include(p => p.ScoreCriterias)
                .ThenInclude(p => p.CriteriaOptions).ToListAsync();
            return _mapper.Map<List<ScoreCriteriaGroupDto>>(groups);
        }

        public async Task<ScoreCriteriaGroupDto> GetGroupCriteria(int criteriaGroupId)
        {
            var groups = await _auow.ScoreCriteriaGroup.All().Include(p => p.ScoreCriterias)
                .ThenInclude(p => p.CriteriaOptions).SingleOrDefaultAsync(p => p.Id == criteriaGroupId);
            return _mapper.Map<ScoreCriteriaGroupDto>(groups);
        }

        public async Task<CandidateScoreDto> EvaluateCandidateCriteria(CandidateDto candidate, int criteriaId)
        {
            string aligoUser = _configuration.GetSection("SystemSettings:SystemUser").Value;
            var criteria = await _auow.ScoreCriteria.All().Include(p => p.CriteriaOptions)
                .Include(p => p.Evaluators).SingleOrDefaultAsync(p => p.Id == criteriaId);
            var criteriaDto = _mapper.Map<ScoreCriteriaDto>(criteria);
            var candidateScore = await Core.EvaluateCandidateCriteriaScore(criteriaDto, candidate, _lifetimeScope);
            if (null == candidateScore)
            {
                return null;
            }
            var scores = new List<CandidateScoreDto>() { candidateScore };
            await UpdateCandidateScores(candidate, scores, aligoUser, false);
            var result = await _auow.CandidateScore.All().SingleOrDefaultAsync(p => p.Candidate.Id == candidate.Id && p.ScoreCriteria.Id == criteriaId);
            return _mapper.Map<CandidateScoreDto>(result);
        }

        public async Task<bool> ResetLock(int candidateId, int criteriaId)
        {
            var result = await _auow.CandidateScore.All().SingleOrDefaultAsync(p => p.Candidate.Id == candidateId && p.ScoreCriteria.Id == criteriaId);
            result.IsLockedForRefresh = !result.IsLockedForRefresh;
            await _auow.SaveChangesAsync();
            return true;
        }

        public async Task<List<CandidateScoreDto>> EvaluateCandidateCriteria(CandidateDto candidate)
        {
            string aligoUser = _configuration.GetSection("SystemSettings:SystemUser").Value;
            //var candidate = _mapper.Map<CandidateDto>(await _auow.Candidate.GetSingleOrDefaultAsync(p => p.Id == candidateId));
            var groups = await _auow.ScoreCriteriaGroup.All().Include(p => p.ScoreCriterias)
                .ThenInclude(p => p.CriteriaOptions).Include(p => p.ScoreCriterias)
                .ThenInclude(p => p.Evaluators).ToListAsync();
            var groupsDto = _mapper.Map<List<ScoreCriteriaGroupDto>>(groups);
            var candidateScores = await Core.EvaluateCandidateScore(groupsDto, candidate, _lifetimeScope);
            await UpdateCandidateScores(candidate, candidateScores, aligoUser, false);
            var scores = await _auow.CandidateScore.All().Where(p => p.Candidate.Id == candidate.Id).ToListAsync();
            return _mapper.Map<List<CandidateScoreDto>>(scores);
        }

        public async Task<bool> SaveCandidateScore(CandidateDto candidate, List<CandidateScoreDto> candidateScores, string updatedBy)
        {
            await UpdateCandidateScores(candidate, candidateScores, updatedBy, true);
            return true;
        }

        public async Task<List<CandidateScoreDto>> GetCandidateScores(int candidateId)
        {
            var scores = await _auow.CandidateScore.All().Where(p => p.Candidate.Id == candidateId)
                .ToListAsync();
            return _mapper.Map<List<CandidateScoreDto>>(scores);

        }

        private async Task UpdateCandidateScores(CandidateDto candidate, List<CandidateScoreDto> candidateScores, string updatedBy, bool refreshLocked)
        {
            var candidateEntity = await _auow.Candidate.GetSingleOrDefaultAsync(p => p.Id == candidate.Id);
            foreach (var score in candidateScores)
            {
                var existing = await _auow.CandidateScore.All().Include(p => p.Candidate)
                    .Include(p => p.ScoreCriteria).FirstOrDefaultAsync(p =>
                        p.Candidate.Id == candidate.Id && p.ScoreCriteria.Id == score.ScoreCriteria.Id);
                if (null == existing)
                {
                    var scoreCriteria =
                        await _auow.ScoreCriteria.GetSingleOrDefaultAsync(p =>
                            p.Id == score.ScoreCriteria.Id);

                    await _auow.CandidateScore.AddAsync(new CandidateScore()
                    {
                        ScoreCriteria = scoreCriteria,
                        Candidate = candidateEntity,
                        CriteriaOptionId = score.CriteriaOptionId,
                        IsLockedForRefresh = refreshLocked,
                        UpdatedBy = updatedBy,
                        CreatedBy = updatedBy
                    });
                }
                else
                {
                    if (existing.IsLockedForRefresh == false || refreshLocked)
                    {
                        existing.CriteriaOptionId = score.CriteriaOptionId;
                        existing.UpdatedBy = updatedBy;
                        existing.IsLockedForRefresh = refreshLocked;
                    }
                }
                await _auow.SaveChangesAsync();
            }
        }
    }
}
