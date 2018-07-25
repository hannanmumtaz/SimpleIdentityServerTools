using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Manager.Client.Results;
using SimpleIdentityServer.Manager.Common.Requests;
using SimpleIdentityServer.Manager.Common.Responses;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Client.Claims
{
    public interface ISearchClaimsOperation
    {
        Task<PagedResult<ClaimResponse>> ExecuteAsync(Uri claimsUri, SearchClaimsRequest parameter, string authorizationHeaderValue = null);
    }

    internal sealed class SearchClaimsOperation : ISearchClaimsOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SearchClaimsOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PagedResult<ClaimResponse>> ExecuteAsync(Uri claimsUri, SearchClaimsRequest parameter, string authorizationHeaderValue = null)
        {
            if (claimsUri == null)
            {
                throw new ArgumentNullException(nameof(claimsUri));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var serializedPostPermission = JsonConvert.SerializeObject(parameter);
            var body = new StringContent(serializedPostPermission, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = claimsUri,
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
                return new PagedResult<ClaimResponse>
                {
                    ContainsError = true,
                    HttpStatus = httpResult.StatusCode,
                    Error = JsonConvert.DeserializeObject<ErrorResponse>(content)
                };
            }

            return new PagedResult<ClaimResponse>
            {
                Content = JsonConvert.DeserializeObject<PagedResponse<ClaimResponse>>(content)
            };
        }
    }
}
