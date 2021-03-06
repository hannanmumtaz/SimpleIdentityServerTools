﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Manager.Common.Responses;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Client.Claims
{
    public interface IAddClaimOperation
    {
        Task<BaseResponse> ExecuteAsync(Uri claimsUri, ClaimResponse claim, string authorizationHeaderValue = null);
    }

    internal sealed class AddClaimOperation : IAddClaimOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AddClaimOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<BaseResponse> ExecuteAsync(Uri claimsUri, ClaimResponse claim, string authorizationHeaderValue = null)
        {
            if (claimsUri == null)
            {
                throw new ArgumentNullException(nameof(claimsUri));
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var serializedJson = JObject.FromObject(claim).ToString();
            var body = new StringContent(serializedJson, Encoding.UTF8, "application/json");
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
            try
            {
                httpResult.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return new BaseResponse
                {
                    ContainsError = true,
                    Error = JsonConvert.DeserializeObject<ErrorResponse>(content)
                };
            }

            return new BaseResponse();
        }
    }
}
