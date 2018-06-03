using SimpleIdentityServer.Parameter.Core.Exceptions;
using System.IO;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Parameter.Core.Parameters.Actions
{
    internal sealed class GetModulesAction
    {
        public async Task Execute()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var configurationPath = Path.Combine(currentDirectory, Constants.ConfigurationFileName);
            var configurationTemplatePath = Path.Combine(currentDirectory, Constants.ConfigurationTemplateFileName);
            if (!File.Exists(configurationPath))
            {
                throw new ConfigurationNotFoundException();
            }

            if (!File.Exists(configurationTemplatePath))
            {
                throw new NotRestoredException();
            }
        }
    }
}
