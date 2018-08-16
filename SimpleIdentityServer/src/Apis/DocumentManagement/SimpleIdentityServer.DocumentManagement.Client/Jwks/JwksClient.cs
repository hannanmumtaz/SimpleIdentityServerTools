using SimpleIdentityServer.Core.Common.DTOs.Requests;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.Jwks
{
    public interface IJwksClient
    {
        Task<JsonWebKeySet> ResolveAsync(string configurationUrl);
        Task<JsonWebKeySet> ResolveAsync(Uri configurationUri);
        Task<JsonWebKeySet> ExecuteAsync(string jwksUrl);
        Task<JsonWebKeySet> ExecuteAsync(Uri jwksUri);
    }

    internal sealed class JwksClient : IJwksClient
    {
        private readonly IGetJwksOperation _getJwksOperation;
        private readonly IGetConfigurationOperation _getConfigurationOperation;

        public JwksClient(IGetJwksOperation getJwksOperation, IGetConfigurationOperation getConfigurationOperation)
        {
            _getJwksOperation = getJwksOperation;
            _getConfigurationOperation = getConfigurationOperation;
        }

        public Task<JsonWebKeySet> ResolveAsync(string configurationUrl)
        {
            if (string.IsNullOrWhiteSpace(configurationUrl))
            {
                throw new ArgumentNullException(nameof(configurationUrl));
            }

            return ResolveAsync(new Uri(configurationUrl));
        }

        public async Task<JsonWebKeySet> ResolveAsync(Uri configurationUri)
        {
            if (configurationUri == null)
            {
                throw new ArgumentNullException(nameof(configurationUri));
            }

            var configuration = await _getConfigurationOperation.Execute(configurationUri).ConfigureAwait(false);
            return await _getJwksOperation.ExecuteAsync(new Uri(configuration.JwksEndpoint)).ConfigureAwait(false);
        }

        public Task<JsonWebKeySet> ExecuteAsync(Uri jwksUri)
        {
            if (jwksUri == null)
            {
                throw new ArgumentNullException(nameof(jwksUri));
            }

            return _getJwksOperation.ExecuteAsync(jwksUri);
        }

        public Task<JsonWebKeySet> ExecuteAsync(string jwksUrl)
        {
            if (string.IsNullOrWhiteSpace(jwksUrl))
            {
                throw new ArgumentNullException(nameof(jwksUrl));
            }

            return ExecuteAsync(new Uri(jwksUrl));
        }
    }
}
