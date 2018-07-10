using Newtonsoft.Json;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Parameter.Core.Common;
using SimpleIdentityServer.Parameter.Core.Exceptions;
using SimpleIdentityServer.Parameter.Core.Helpers;
using SimpleIdentityServer.Parameter.Core.Responses;
using System.IO;

namespace SimpleIdentityServer.Parameter.Core.Parameters.Actions
{
    public interface IGetUnitsAction
    {
        GetModulesResponse Execute();
    }

    internal sealed class GetUnitsAction : IGetUnitsAction
    {
        private readonly IGetProjectConfiguration _getProjectConfiguration;

        public GetUnitsAction(IGetProjectConfiguration getProjectConfiguration)
        {
            _getProjectConfiguration = getProjectConfiguration;
        }

        public GetModulesResponse Execute()
        {
            var result = _getProjectConfiguration.Execute();
            return new GetModulesResponse
            {
                ProjectUnits = result.Key.Units,
                ProjectTemplateUnits = result.Value.Units
            };
        }
    }
}
