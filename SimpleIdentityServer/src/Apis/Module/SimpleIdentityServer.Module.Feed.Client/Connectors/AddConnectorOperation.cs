using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Module.Feed.Common.Requests;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Client.Connectors
{
    public interface IAddConnectorOperation
    {
        Task<BaseResponse> Execute(AddConnectorRequest request, string url);
    }

    internal sealed class AddConnectorOperation : IAddConnectorOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AddConnectorOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<BaseResponse> Execute(AddConnectorRequest request, string url)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var serializedJson = JObject.FromObject(request).ToString();
            var body = new StringContent(serializedJson, Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = body,
                RequestUri = new Uri(url)
            };
            var httpResponse = await httpClient.SendAsync(httpRequest).ConfigureAwait(false);
            try
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (Exception)
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
