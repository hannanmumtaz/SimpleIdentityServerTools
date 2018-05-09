using System;

namespace RpEid.Api.Aggregates
{
    public class AccountAggregate
    {
        public string Subject { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsGranted { get; set; }
        public bool IsConfirmed { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? GrantDateTime { get; set; }
        public DateTime? ConfirmationDateTime { get; set; }
    }
}
