using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Client.Connectors
{
    public interface IDeleteConnectorOperation
    {
        Task<BaseResponse> Execute(string id, string url);
    }

    internal sealed class DeleteConnectorOperation : IDeleteConnectorOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteConnectorOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<BaseResponse> Execute(string id, string url)
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
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{url}/{id}")
            };
            var httpResponse = await httpClient.SendAsync(request).ConfigureAwait(false);
            try
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            catch(Exception)
            {
                ErrorResponse error = null;
                var json = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    error = JsonConvert.DeserializeObject<ErrorResponse>(json);
                }

                return new BaseResponse
                {
                    ContainsError = false,
                    Error = error,
                    HttpStatus = httpResponse.StatusCode
                };
            }

            return new BaseResponse
            {
                ContainsError = false
            };
        }
    }
}
