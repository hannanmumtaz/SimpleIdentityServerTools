using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.ResourceManager.Client.Responses;
using SimpleIdentityServer.ResourceManager.Common.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Client.HierarchicalResources
{
    public interface IGetHierarchicalResourceOperation
    {
        Task<GetHierarchicalResourceResponse> Execute(string url, string name, bool includeChildren, string authorizationHeaderValue = null);
    }

    internal sealed class GetHierarchicalResourceOperation  : IGetHierarchicalResourceOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetHierarchicalResourceOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetHierarchicalResourceResponse> Execute(string url, string name, bool includeChildren, string authorizationHeaderValue = null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var uri = new Uri($"{url}/{name}/{includeChildren}");
            var httpClient = _httpClientFactory.GetHttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };
            if (!string.IsNullOrWhiteSpace(authorizationHeaderValue))
            {
                request.Headers.Add("Authorization", "Bearer " + authorizationHeaderValue);
            }

            var httpResult = await httpClient.SendAsync(request).ConfigureAwait(false);
            var json = await httpResult.Content.ReadAsStringAsync();
            try
            {
                httpResult.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException)
            {
                var rec = JsonConvert.DeserializeObject<ErrorResponse>(json);
                return new GetHierarchicalResourceResponse
                {
                    ContainsError = true,
                    Error = rec
                };
            }
            catch(Exception)
            {
                return new GetHierarchicalResourceResponse
                {
                    ContainsError = true
                };
            }

            var assets = JsonConvert.DeserializeObject<IEnumerable<AssetResponse>>(json);
            return new GetHierarchicalResourceResponse
            {
                Content = assets
            };
        }
    }
}
