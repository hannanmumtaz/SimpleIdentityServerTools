using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Core.Common.DTOs.Responses;
using SimpleIdentityServer.Manager.Client.Factories;
using SimpleIdentityServer.Manager.Client.Results;
using SimpleIdentityServer.Manager.Common.Requests;
using SimpleIdentityServer.Manager.Common.Responses;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Client.Clients
{
    public interface ISearchClientOperation
    {
        Task<PagedResult<ClientResponse>> ExecuteAsync(Uri clientsUri, SearchClientsRequest parameter, string authorizationHeaderValue = null);
    }

    internal sealed class SearchClientOperation : ISearchClientOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SearchClientOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PagedResult<ClientResponse>> ExecuteAsync(Uri clientsUri, SearchClientsRequest parameter, string authorizationHeaderValue = null)
        {
            if (clientsUri == null)
            {
                throw new ArgumentNullException(nameof(clientsUri));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var serializedPostPermission = JsonConvert.SerializeObject(parameter);
            var body = new StringContent(serializedPostPermission, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = clientsUri,
                Content = body
            };
            if (!string.IsNullOrWhiteSpace(authorizationHeaderValue))
            {
                request.Headers.Add("Authorization", "Bearer " + authorizationHeaderValue);
            }

            var httpResult = await httpClient.SendAsync(request);
            var content = await httpResult.Content.ReadAsStringAsync().ConfigureAwait(false);
            var rec = JObject.Parse(content);
            try
            {
                httpResult.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                var result = new PagedResult<ClientResponse>
                {
                    ContainsError = true,
                    Status = httpResult.StatusCode
                };
                if (!string.IsNullOrWhiteSpace(content))
                {
                    result.Error = JsonConvert.DeserializeObject<ErrorResponseWithState>(content);
                }

                return result;
            }

            return new PagedResult<ClientResponse>
            {
                Content = JsonConvert.DeserializeObject<PagedResponse<ClientResponse>>(content)
            };
        }
    }
}
