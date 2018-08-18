using System;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses
{
    [DataContract]
    public class OfficeDocumentInvitationLinkResponse
    {
        [DataMember(Name = "confirmation_code")]
        public string ConfirmationCode { get; set; }
        [DataMember(Name = "documentid")]
        public string DocumentId { get; set; }
        [DataMember(Name = "exp")]
        public int? ExpiresIn { get; set; }
        [DataMember(Name = "nb")]
        public int? NumberOfConfirmations { get; set; }
        [DataMember(Name = "create_datetime")]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = "update_datetime")]
        public DateTime UpdateDateTime { get; set; }
    }
}
