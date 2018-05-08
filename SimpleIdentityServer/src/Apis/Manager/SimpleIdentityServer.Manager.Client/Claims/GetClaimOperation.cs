using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Manager.Client.Factories;
using SimpleIdentityServer.Manager.Client.DTOs.Responses;
using SimpleIdentityServer.Manager.Common.Responses;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Client.Claims
{
    public interface IGetClaimOperation
    {
        Task<GetClaimResponse> ExecuteAsync(Uri claimsUri, string authorizationHeaderValue = null);
    }

    internal sealed class GetClaimOperation : IGetClaimOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetClaimOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetClaimResponse> ExecuteAsync(Uri claimsUri, string authorizationHeaderValue = null)
        {
            if (claimsUri == null)
            {
                throw new ArgumentNullException(nameof(claimsUri));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = claimsUri
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
            catch (HttpRequestException)
            {
                var err = JsonConvert.DeserializeObject<ErrorResponse>(content);
                return new GetClaimResponse(err);
            }
            catch (Exception)
            {
                return new GetClaimResponse
                {
                    ContainsError = true
                };
            }

            var client = JsonConvert.DeserializeObject<ClaimResponse>(content);
            return new GetClaimResponse
            {
                Content = client
            };
        }
    }
}
