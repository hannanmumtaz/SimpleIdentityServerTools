using System;
using System.Runtime.Serialization;

namespace RpEid.Api.DTOs.Responses
{
    [DataContract]
    public class AccountResponse
    {
        [DataMember(Name = Constants.AccountNames.Subject)]
        public string Subject { get; set; }
        [DataMember(Name = Constants.AccountNames.Name)]
        public string Name { get; set; }
        [DataMember(Name = Constants.AccountNames.Email)]
        public string Email { get; set; }
        [DataMember(Name = Constants.AccountNames.IsConfirmed)]
        public bool IsConfirmed { get; set; }
        [DataMember(Name = Constants.AccountNames.CreateDateTime)]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = Constants.AccountNames.UpdateDateTime)]
        public DateTime UpdateDateTime { get; set; }
    }
}
