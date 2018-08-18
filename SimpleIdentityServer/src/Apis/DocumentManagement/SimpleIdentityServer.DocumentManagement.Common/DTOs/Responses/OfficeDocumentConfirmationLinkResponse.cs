using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses
{
    [DataContract]
    public class OfficeDocumentConfirmationLinkResponse
    {
        [DataMember(Name = "confirmation_code")]
        public string ConfirmationCode { get; set; }
        [DataMember(Name = "redirect_url")]
        public string Url { get; set; }
    }
}
