using Newtonsoft.Json;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Parameter.Core.Helpers;
using SimpleIdentityServer.Parameter.Core.Params;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleIdentityServer.Parameter.Core.Parameters.Actions
{
    public interface IUpdateTwoFactorsAction
    {
        bool Execute(IEnumerable<UpdateTwoFactor> updateTwoFactors);
    }

    internal sealed class UpdateTwoFactorsAction : IUpdateTwoFactorsAction
    {
        private readonly IDirectoryHelper _directoryHelper;

        public UpdateTwoFactorsAction(IDirectoryHelper directoryHelper)
        {
            _directoryHelper = directoryHelper;
        }

        public bool Execute(IEnumerable<UpdateTwoFactor> updateTwoFactors)
        {
            if (updateTwoFactors == null)
            {
                throw new ArgumentNullException(nameof(updateTwoFactors));
            }

            var currentDirectory = _directoryHelper.GetCurrentDirectory();
            var configurationPath = Path.Combine(currentDirectory, Constants.ConfigurationFileName);
            var projectConfiguration = JsonConvert.DeserializeObject<ProjectResponse>(File.ReadAllText(configurationPath));
            var twoFactors = new List<ProjectTwoFactorAuthenticator>();
            foreach (var updateTwoFactor in updateTwoFactors)
            {
                twoFactors.Add(new ProjectTwoFactorAuthenticator
                {
                    Library = updateTwoFactor.Library,
                    Name = updateTwoFactor.Name,
                    Parameters = updateTwoFactor.Parameters,
                    Version = updateTwoFactor.Version
                });
            }

            projectConfiguration.TwoFactors = twoFactors;
            var json = JsonConvert.SerializeObject(projectConfiguration);
            File.WriteAllText(configurationPath, json);
            return true;
        }
    }
}
