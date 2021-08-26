using Alifery.Connect.CoreEntities;

namespace Alifery.Connect.Communications.Model
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public MediaDto ProfilePicture { get; set; }

    }
}