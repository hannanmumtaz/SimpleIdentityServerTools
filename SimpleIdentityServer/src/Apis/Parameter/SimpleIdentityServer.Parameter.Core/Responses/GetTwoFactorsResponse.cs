using SimpleIdentityServer.Module.Feed.Common.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.Parameter.Core.Responses
{
    public class GetTwoFactorsResponse
    {
        public GetTwoFactorsResponse()
        {
            ProjectTwoFactors = new List<ProjectTwoFactorAuthenticator>();
            ProjectTemplateTwoFactors = new List<ProjectTwoFactorAuthenticator>();
        }

        public IEnumerable<ProjectTwoFactorAuthenticator> ProjectTwoFactors { get; set; }
        public IEnumerable<ProjectTwoFactorAuthenticator> ProjectTemplateTwoFactors { get; set; }
    }
}