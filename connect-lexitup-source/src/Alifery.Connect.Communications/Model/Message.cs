using System;
using System.Collections.Generic;

namespace Alifery.Connect.Communications.Model
{
    public class Message
    {
        public User User { get; set; }
        public Guid ChatId { get; set; }
        public IEnumerable<User> OtherUsers { get; set; }
        public string Text { get; set; }
    }
}