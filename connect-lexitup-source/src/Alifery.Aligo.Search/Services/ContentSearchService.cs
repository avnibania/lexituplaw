using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alifery.Aligo.Search.Core;
using Alifery.Aligo.Search.Models;
using Alifery.Connect.CoreEntities.AligoEntities.Candidate;
using Alifery.Connect.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Alifery.Aligo.Search.Services
{
    public class ContentSearchService : IContentSearchService
    {
        private readonly IAligoUnitOfWork _auow;
        private readonly IMapper _mapper;

        public ContentSearchService(IMapper mapper, IAligoUnitOfWork auow)
        {
            _mapper = mapper;
            _auow = auow;
        }

        public async Task<ICollection<SearchResultModel>> SearchCandidates(string searchString,
            List<CandidateSearchInput> input = null)
        {
            var result = new List<SearchResultModel>();
            var candidateContents = await _auow.CandidateContent.All().Include(p => p.Candidate).ToListAsync();
            if (null != candidateContents)
                foreach (var content in candidateContents)
                    if (content.Content.ExactMatch(searchString))
                    {
                        var contentDto = _mapper.Map<CandidateContentDto>(content);
                        var candidateDto = _mapper.Map<CandidateDto>(content.Candidate);
                        var noOfOccurrences = content.Content.Split(searchString, StringSplitOptions.RemoveEmptyEntries)
                            .Length;
                        if (result.Any(p => p.Candidate.Id == content.CandidateId))
                        {
                            result.FirstOrDefault(p => p.Candidate.Id == content.CandidateId)
                                ?.AddContent(contentDto, noOfOccurrences);
                        }
                        else
                        {
                            var contentResult = new SearchResultModel(candidateDto);
                            contentResult.AddContent(contentDto, noOfOccurrences);
                            result.Add(contentResult);
                        }
                    }

            if (null == input)
                return result;

            foreach (var content in input)
            {
                if (string.IsNullOrEmpty(content?.Content))
                    continue;
                if (content.Content.ExactMatch(searchString))
                {
                    var candidate = await _auow.Candidate.All()
                        .FirstOrDefaultAsync(p => p.AliferyFreelancerId == content.aliferyFreelancerId);
                    if (null == candidate)
                        continue;
                    var contentDto = new CandidateContentDto
                    {
                        Content = content.Content,
                        ContentSource = content.ContentSource,
                        CandidateId = candidate.Id
                    };
                    var candidateDto = _mapper.Map<CandidateDto>(candidate);
                    var noOfOccurrences = content.Content.Split(searchString, StringSplitOptions.RemoveEmptyEntries)
                        .Length;
                    if (result.Any(p => p.Candidate.Id == candidate.Id))
                    {
                        result.FirstOrDefault(p => p.Candidate.Id == candidate.Id)
                            ?.AddContent(contentDto, noOfOccurrences);
                    }
                    else
                    {
                        var contentResult = new SearchResultModel(candidateDto);
                        contentResult.AliferyFreelancerId = content.aliferyFreelancerId;
                        contentResult.AddContent(contentDto, noOfOccurrences);
                        result.Add(contentResult);
                    }
                }
            }

            return result;
        }
    }
}