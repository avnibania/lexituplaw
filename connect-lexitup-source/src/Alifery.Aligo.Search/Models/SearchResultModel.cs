using System;
using System.Collections.Generic;
using Alifery.Connect.CoreEntities.AligoEntities.Candidate;

namespace Alifery.Aligo.Search.Models
{
    public class SearchResultModel
    {
        public SearchResultModel()
        {
            Contents = new List<CandidateContentDto>();
            SearchTime = DateTime.UtcNow;
        }

        public SearchResultModel(CandidateDto candidate)
        {
            Candidate = candidate;
            SearchTime = DateTime.UtcNow;
        }

        public CandidateDto Candidate { get; set; }
        public int AliferyFreelancerId { get; set; }
        public int NoOfOccurrences { get; set; }
        public List<CandidateContentDto> Contents { get; set; }
        public DateTime SearchTime { get; set; }
        public void AddContent(CandidateContentDto content, int noOfOccurrences)
        {
            if (null == Contents)
            {
                Contents = new List<CandidateContentDto>();
                NoOfOccurrences = 0;
            }
            NoOfOccurrences += noOfOccurrences;
            Contents.Add(content);
        }
    }
}