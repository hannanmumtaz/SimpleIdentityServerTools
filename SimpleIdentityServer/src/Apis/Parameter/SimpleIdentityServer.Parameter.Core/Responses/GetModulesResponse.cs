using SimpleIdentityServer.Module.Feed.Common.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.Parameter.Core.Responses
{
    public class GetModulesResponse
    {
        public GetModulesResponse()
        {
            ProjectUnits = new List<ProjectUnitResponse>();
            ProjectTemplateUnits = new List<ProjectUnitResponse>();
        }

        public IEnumerable<ProjectUnitResponse> ProjectUnits { get; set; }
        public IEnumerable<ProjectUnitResponse> ProjectTemplateUnits { get; set; }
    }
}
