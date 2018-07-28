using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Core.Common.DTOs.Responses;
using SimpleIdentityServer.Manager.Client.Results;
using SimpleIdentityServer.Manager.Common.Requests;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Client.Clients
{
    public interface IAddClientOperation
    {
        Task<AddClientResult> ExecuteAsync(Uri clientsUri, AddClientRequest client, string authorizationHeaderValue = null);
    }

    internal sealed class AddClientOperation : IAddClientOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AddClientOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AddClientResult> ExecuteAsync(Uri clientsUri, AddClientRequest client, string authorizationHeaderValue = null)
        {
            if (clientsUri == null)
            {
                throw new ArgumentNullException(nameof(clientsUri));
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var serializedJson = JObject.FromObject(client).ToString();
            var body = new StringContent(serializedJson, Encoding.UTF8, "application/json");
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
            try
            {
                httpResult.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return new AddClientResult
                {
                    ContainsError = true,
                    Error = JsonConvert.DeserializeObject<ErrorResponse>(content),
                    HttpStatus = httpResult.StatusCode
                };
            }
            
            return new AddClientResult
            {
                Content = JsonConvert.DeserializeObject<ClientRegistrationResponse>(content)
            };
        }
    }
}
