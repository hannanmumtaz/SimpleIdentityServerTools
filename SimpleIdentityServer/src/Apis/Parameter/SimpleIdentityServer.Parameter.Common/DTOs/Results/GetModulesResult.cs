using SimpleIdentityServer.Module.Feed.Common.Responses;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Parameter.Common.DTOs.Results
{
    [DataContract]
    public class GetModulesResult
    {
        [DataMember(Name = "units")]
        public IEnumerable<ProjectUnitResponse> Units { get; set; }
        [DataMember(Name = "template_units")]
        public IEnumerable<ProjectUnitResponse> TemplateUnits { get; set; }
    }
}