using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses
{
    [DataContract]
    public class SearchOfficeDocumentsResponse
    {
        [DataMember(Name = "total_results")]
        public int TotalResults { get; set; }
        [DataMember(Name = "content")]
        public IEnumerable<OfficeDocumentResponse> Content { get; set; }
    }
}