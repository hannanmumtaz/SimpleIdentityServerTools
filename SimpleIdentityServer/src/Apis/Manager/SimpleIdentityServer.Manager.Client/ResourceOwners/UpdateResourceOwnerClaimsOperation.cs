using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Manager.Common.Requests;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Client.ResourceOwners
{
    public interface IUpdateResourceOwnerClaimsOperation
    {
        Task<BaseResponse> ExecuteAsync(Uri resourceOwnerUri, UpdateResourceOwnerClaimsRequest updateResourceOwnerClaimsRequest, string authorizationHeaderValue = null);
    }

    internal sealed class UpdateResourceOwnerClaimsOperation : IUpdateResourceOwnerClaimsOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UpdateResourceOwnerClaimsOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<BaseResponse> ExecuteAsync(Uri resourceOwnerUri, UpdateResourceOwnerClaimsRequest updateResourceOwnerClaimsRequest, string authorizationHeaderValue = null)
        {
            if (resourceOwnerUri == null)
            {
                throw new ArgumentNullException(nameof(resourceOwnerUri));
            }

            if (updateResourceOwnerClaimsRequest == null)
            {
                throw new ArgumentNullException(nameof(updateResourceOwnerClaimsRequest));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var serializedJson = JsonConvert.SerializeObject(updateResourceOwnerClaimsRequest).ToString();
            var body = new StringContent(serializedJson, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = resourceOwnerUri,
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
                return new BaseResponse
                {
                    ContainsError = true,
                    Error = JsonConvert.DeserializeObject<ErrorResponse>(content),
                    HttpStatus = httpResult.StatusCode
                };
            }

            return new BaseResponse();
        }
    }
}
