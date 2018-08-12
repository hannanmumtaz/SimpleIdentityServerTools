using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Manager.Client.Results;
using SimpleIdentityServer.Manager.Common.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Client.Claims
{
    public interface IGetAllClaimsOperation
    {
        Task<GetAllClaimsResult> ExecuteAsync(Uri claimsUri, string authorizationHeaderValue = null);
    }

    internal sealed class GetAllClaimsOperation : IGetAllClaimsOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetAllClaimsOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetAllClaimsResult> ExecuteAsync(Uri claimsUri, string authorizationHeaderValue = null)
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
            try
            {
                httpResult.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return new GetAllClaimsResult
                {
                    ContainsError = true,
                    Error = JsonConvert.DeserializeObject<ErrorResponse>(content),
                    HttpStatus = httpResult.StatusCode
                };
            }

            return new GetAllClaimsResult
            {
                Content = JsonConvert.DeserializeObject<IEnumerable<ClaimResponse>>(content)
            };
        }
    }
}
