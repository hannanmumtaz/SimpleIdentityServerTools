using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests
{
    [DataContract]
    public class SearchOfficeDocumentRequest
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "start_index")]
        public int StartIndex { get; set; }
    }
}
