using System;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.License
{
    [DataContract]
    public class LicenseFile
    {
        [DataMember(Name = "organisation")]
        public string Organisation { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "issue_datetime")]
        public DateTime IssueDateTime { get; set; }
        [DataMember(Name = "expiration_datetime")]
        public DateTime ExpirationDateTime { get; set; }
    }
}
