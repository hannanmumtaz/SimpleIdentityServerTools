using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Module.Feed.Client.Results;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Client.Connectors
{
    public interface IGetAllConnectorsOperation
    {
        Task<GetAllConnectorsResult> Execute(string url);
    }

    internal sealed class GetAllConnectorsOperation : IGetAllConnectorsOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetAllConnectorsOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetAllConnectorsResult> Execute(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            var httpResponse = await httpClient.SendAsync(request).ConfigureAwait(false);
            var json = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            try
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                ErrorResponse error = null;
                if (!string.IsNullOrWhiteSpace(json))
                {
                    error = JsonConvert.DeserializeObject<ErrorResponse>(json);
                }

                return new GetAllConnectorsResult
                {
                    ContainsError = false,
                    Error = error,
                    HttpStatus = httpResponse.StatusCode
                };
            }

            return new GetAllConnectorsResult
            {
                Content = JsonConvert.DeserializeObject<IEnumerable<ConnectorResponse>>(json)
            };
        }
    }
}
