using Alifery.Connect.CoreEntities.AligoEntities.Search;

namespace Alifery.Aligo.Search.Models
{
    public class CandidateContentDto
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public string Content { get; set; }
        public ContentSource ContentSource { get; set; }
    }
}