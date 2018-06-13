using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Parameter.Common.DTOs.Results;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Parameter.Client
{
    public interface IGetConnectorsAction
    {
        Task<GetConnectorsResult> Execute(string baseUrl, string accessToken = null);
    }

    internal sealed class GetConnectorsAction : IGetConnectorsAction
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetConnectorsAction(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetConnectorsResult> Execute(string baseUrl, string accessToken = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{baseUrl}/parameters/connectors")
            };
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.Headers.Add("Authorization", $"Bearer {accessToken}");
            }

            var httpResponse = await httpClient.SendAsync(request).ConfigureAwait(false);
            httpResponse.EnsureSuccessStatusCode();
            var json = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetConnectorsResult>(json);
        }
    }
}
