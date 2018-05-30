
using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Client
{
    public interface IConfigurationClient
    {
        Task<ConfigurationResponse> GetConfiguration(string url);
    }

    internal sealed class ConfigurationClient : IConfigurationClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ConfigurationClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ConfigurationResponse> GetConfiguration(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var client = _httpClientFactory.GetHttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            var httpResponse = await client.SendAsync(request).ConfigureAwait(false);
            httpResponse.EnsureSuccessStatusCode();
            var json = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ConfigurationResponse>(json);
        }
    }
}
