using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.DocumentManagement.Client.Responses;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments
{
    public interface IGetPermissionsOperation
    {
        Task<GetOfficeDocumentPermissionsResponse> Execute(string documentId, string url, string accessToken);
    }

    internal sealed class GetPermissionsOperation : IGetPermissionsOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetPermissionsOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetOfficeDocumentPermissionsResponse> Execute(string documentId, string url, string accessToken)
        {
            if (string.IsNullOrWhiteSpace(documentId))
            {
                throw new ArgumentNullException(nameof(documentId));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{url}/{documentId}/permissions")
            };
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                httpRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            }

            var httpResponse = await httpClient.SendAsync(httpRequest).ConfigureAwait(false);
            var json = await httpResponse.Content.ReadAsStringAsync();
            try
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                var result = new GetOfficeDocumentPermissionsResponse
                {
                    HttpStatus = httpResponse.StatusCode,
                    ContainsError = true,
                    Error = TryGetError(json)
                };

                if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    IEnumerable<string> values;
                    if (httpResponse.Headers.TryGetValues("UmaResource", out values))
                    {
                        result.UmaResourceId = values.First();
                    }

                    if (httpResponse.Headers.TryGetValues("UmaWellKnownUrl", out values))
                    {
                        result.UmaWellKnownUrl = values.First();
                    }
                }

                return result;
            }

            return new GetOfficeDocumentPermissionsResponse
            {
                ContainsError = false,
                Content = JsonConvert.DeserializeObject<IEnumerable<OfficeDocumentPermissionResponse>>(json)
            };
        }

        private static string TryGetValue(HttpResponseHeaders header, string name)
        {
            IEnumerable<string> values;
            if (header.TryGetValues(name, out values))
            {
                return values.First();
            }

            return null;
        }

        private static ErrorResponse TryGetError(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<ErrorResponse>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}