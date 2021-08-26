using Alifery.Connect.CoreEntities.Enums;
using System;
using System.Collections.Generic;

namespace Alifery.Connect.Communications.Model
{
    public class NotificationOptions
    {
        public Notification Notification { get; set; }
        public User User { get; set; }
        public ICollection<User> NotifyUsers { get; set; }
    }

    public class Notification
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsRead { get; set; }
        public NotificationGroup Group { get; set; }
        public string AdditionalData { get; set; }
    }

    public class NotificationAdditionalData
    {
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
        public int InvoiceId { get; set; }
        public int FreelancerId { get; set; }
        public string ProjectLink { get; set; }
        public string ClientLink { get; set; }
        public string InvoiceLink { get; set; }
        public int FreelancerLink { get; set; }
        public string ProjectName { get; set; }
        public string ClientName { get; set; }
        public string FreelancerName { get; set; }
    }
}
