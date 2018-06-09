using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests
{
    [DataContract]
    public class AddOfficeDocumentRequest
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "enc_alg")]
        public OfficeDocumentEncAlgorithms? EncAlg { get; set; }
        [DataMember(Name = "enc_password")]
        public string EncPassword { get; set; }
        [DataMember(Name = "enc_salt")]
        public string EncSalt { get; set; }
    }
}
