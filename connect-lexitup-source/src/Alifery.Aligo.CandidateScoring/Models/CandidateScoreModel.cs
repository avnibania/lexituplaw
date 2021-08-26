namespace Alifery.Aligo.CandidateScoring.Models
{
    public class CandidateScoreModel
    {
        public int FreelancerId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Criteria { get; set; }
        public string Score { get; set; }
        public bool Evaluated { get; set; }
    }
}
