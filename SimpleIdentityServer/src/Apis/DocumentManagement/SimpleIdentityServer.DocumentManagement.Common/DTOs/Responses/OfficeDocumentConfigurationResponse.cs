using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses
{
    [DataContract]
    public class OfficeDocumentConfigurationResponse
    {
        [DataMember(Name = "jwks_endpoint")]
        public string JwksEndpoint { get; set; }
        [DataMember(Name = "office_documents_endpoint")]
        public string OfficeDocumentsEndpoint { get; set; }
    }
}
