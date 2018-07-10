using SimpleIdentityServer.Parameter.Core.Common;
using SimpleIdentityServer.Parameter.Core.Responses;

namespace SimpleIdentityServer.Parameter.Core.Parameters.Actions
{
    public interface IGetTwoFactorsAction
    {
        GetTwoFactorsResponse Execute();
    }

    internal sealed class GetTwoFactorsAction : IGetTwoFactorsAction
    {
        private readonly IGetProjectConfiguration _getProjectConfiguration;

        public GetTwoFactorsAction(IGetProjectConfiguration getProjectConfiguration)
        {
            _getProjectConfiguration = getProjectConfiguration;
        }
        
        public GetTwoFactorsResponse Execute()
        {
            var result = _getProjectConfiguration.Execute();
            return new GetTwoFactorsResponse
            {
                ProjectTwoFactors = result.Key.TwoFactors,
                ProjectTemplateTwoFactors = result.Value.TwoFactors
            };
        }
    }
}
