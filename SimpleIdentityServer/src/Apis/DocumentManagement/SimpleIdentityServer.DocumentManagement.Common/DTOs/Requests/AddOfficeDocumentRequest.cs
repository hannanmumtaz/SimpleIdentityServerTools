using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests
{
    [DataContract]
    public class AddOfficeDocumentRequest
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "public_key")]
        public string PublicKey { get; set; }
        [DataMember(Name = "private_key")]
        public string PrivateKey { get; set; }
    }
}
