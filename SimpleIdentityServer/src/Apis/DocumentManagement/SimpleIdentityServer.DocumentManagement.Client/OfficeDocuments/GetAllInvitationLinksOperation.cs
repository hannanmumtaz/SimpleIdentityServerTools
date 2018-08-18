using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.DocumentManagement.Client.Responses;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments
{
    public interface IGetAllInvitationLinksOperation
    {
        Task<GetAllInvitationLinksResponse> Execute(string documentId, string url, string accessToken);
    }

    internal sealed class GetAllInvitationLinksOperation : IGetAllInvitationLinksOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetAllInvitationLinksOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetAllInvitationLinksResponse> Execute(string documentId, string url, string accessToken)
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
                RequestUri = new Uri($"{url}/{documentId}/invitation")
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
                return new GetAllInvitationLinksResponse
                {
                    HttpStatus = httpResponse.StatusCode,
                    ContainsError = true,
                    Error = TryGetError(json)
                };
            }

            return new GetAllInvitationLinksResponse
            {
                ContainsError = false,
                Content = JsonConvert.DeserializeObject<IEnumerable<OfficeDocumentInvitationLinkResponse>>(json)
            };
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
