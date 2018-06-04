using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Parameter.Common.DTOs.Requests
{
    [DataContract]
    public class UpdateParameterRequest
    {
        [DataMember(Name = "name")]
        public string UnitName { get; set; }
        [DataMember(Name = "category")]
        public string CategoryName { get; set; }
        [DataMember(Name = "lib")]
        public string LibraryName { get; set; }
        [DataMember(Name = "parameters")]
        public Dictionary<string, string> Parameters { get; set; }
    }
}
