using Alifery.Connect.CoreEntities.AligoEntities.Search;

namespace Alifery.Aligo.Search.Models
{
    public class CandidateSearchInput
    {
        public int aliferyFreelancerId { get; set; }
        public string Content { get; set; }
        public ContentSource ContentSource { get; set; }
    }
}