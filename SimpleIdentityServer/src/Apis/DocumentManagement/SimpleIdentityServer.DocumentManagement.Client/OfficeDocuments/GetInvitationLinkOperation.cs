using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.DocumentManagement.Client.Responses;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments
{
    public interface IGetInvitationLinkOperation
    {
        Task<GetInvitationLinkResponse> Execute(string documentId, GenerateConfirmationCodeRequest request, string url, string accessToken);
    }

    internal sealed class GetInvitationLinkOperation : IGetInvitationLinkOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetInvitationLinkOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetInvitationLinkResponse> Execute(string documentId, GenerateConfirmationCodeRequest request, string url, string accessToken)
        {
            if (string.IsNullOrWhiteSpace(documentId))
            {
                throw new ArgumentNullException(nameof(documentId));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var serializedPostPermission = JsonConvert.SerializeObject(request);
            var body = new StringContent(serializedPostPermission, Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = body,
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
                return new GetInvitationLinkResponse
                {
                    HttpStatus = httpResponse.StatusCode,
                    ContainsError = true,
                    Error = TryGetError(json)
                };
            }

            return new GetInvitationLinkResponse
            {
                ContainsError = false,
                Content = JsonConvert.DeserializeObject<OfficeDocumentConfirmationLinkResponse>(json)
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
