using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.ResourceManager.Common.Responses;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Client.Configuration
{
    public interface IGetConfigurationOperation
    {
        Task<ConfigurationResponse> Execute(Uri uri);
    }

    internal sealed class GetConfigurationOperation : IGetConfigurationOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetConfigurationOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ConfigurationResponse> Execute(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }
            
            var httpClient = _httpClientFactory.GetHttpClient();
            var serializedContent = await httpClient.GetStringAsync(uri).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ConfigurationResponse>(serializedContent);
        }
    }
}
