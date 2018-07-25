using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Manager.Client.Results;
using SimpleIdentityServer.Manager.Common.Responses;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Client.Configuration
{
    public interface IGetConfigurationOperation
    {
        Task<GetConfigurationResult> ExecuteAsync(Uri wellKnownConfigurationUrl);
    }

    internal sealed class GetConfigurationOperation : IGetConfigurationOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetConfigurationOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetConfigurationResult> ExecuteAsync(Uri wellKnownConfigurationUri)
        {
            if (wellKnownConfigurationUri == null)
            {
                throw new ArgumentNullException(nameof(wellKnownConfigurationUri));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = wellKnownConfigurationUri
            };
            var httpResult = await httpClient.SendAsync(request);
            httpResult.EnsureSuccessStatusCode();
            var content = await httpResult.Content.ReadAsStringAsync();
            try
            {
                httpResult.EnsureSuccessStatusCode();
            }
            catch(Exception)
            {
                return new GetConfigurationResult
                {
                    ContainsError = true,
                    HttpStatus = httpResult.StatusCode
                };
            }

            return new GetConfigurationResult
            {
                Content = JsonConvert.DeserializeObject<ConfigurationResponse>(content)
            };
        }
    }
}
