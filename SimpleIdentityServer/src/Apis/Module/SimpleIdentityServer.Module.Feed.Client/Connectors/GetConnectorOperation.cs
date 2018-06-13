using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Module.Feed.Client.Results;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Client.Connectors
{
    public interface IGetConnectorOperation
    {
        Task<GetConnectorResult> Execute(string id, string url);
    }

    internal sealed class GetConnectorOperation : IGetConnectorOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetConnectorOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetConnectorResult> Execute(string id, string url)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{url}/{id}")
            };
            var httpResponse = await httpClient.SendAsync(request).ConfigureAwait(false);
            var json = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            try
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                ErrorResponse error = null;
                if (!string.IsNullOrWhiteSpace(json))
                {
                    error = JsonConvert.DeserializeObject<ErrorResponse>(json);
                }

                return new GetConnectorResult
                {
                    ContainsError = false,
                    Error = error,
                    HttpStatus = httpResponse.StatusCode
                };
            }

            return new GetConnectorResult
            {
                Content = JsonConvert.DeserializeObject<ConnectorResponse>(json)
            };
        }
    }
}
