using SimpleIdentityServer.Module.Feed.Common.Responses;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Parameter.Common.DTOs.Results
{
    [DataContract]
    public class GetTwoFactorsResult
    {
        [DataMember(Name = "twofactors")]
        public IEnumerable<ProjectTwoFactorAuthenticator> TwoFactors { get; set; }
        [DataMember(Name = "template_twofactors")]
        public IEnumerable<ProjectTwoFactorAuthenticator> TemplateTwoFactors { get; set; }
    }
}
