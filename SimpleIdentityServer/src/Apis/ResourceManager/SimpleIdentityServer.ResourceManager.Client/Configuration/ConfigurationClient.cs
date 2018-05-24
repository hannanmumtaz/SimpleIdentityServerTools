using SimpleIdentityServer.ResourceManager.Common.Responses;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Client.Configuration
{
    public interface IConfigurationClient
    {
        Task<ConfigurationResponse> GetConfiguration(Uri uri);
    }

    internal class ConfigurationClient : IConfigurationClient
    {
        private readonly IGetConfigurationOperation _getConfigurationOperation;

        public ConfigurationClient(IGetConfigurationOperation getConfigurationOperation)
        {
            _getConfigurationOperation = getConfigurationOperation;
        }

        public Task<ConfigurationResponse> GetConfiguration(Uri uri)
        {
            return _getConfigurationOperation.Execute(uri);
        }
    }
}
