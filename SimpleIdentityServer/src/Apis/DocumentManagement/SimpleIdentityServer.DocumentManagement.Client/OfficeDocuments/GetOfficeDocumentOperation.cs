using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.DocumentManagement.Client.Responses;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments
{
    public interface IGetOfficeDocumentOperation
    {
        Task<GetOfficeDocumentResponse> Execute(string documentId, string url, string accessToken);
    }

    internal sealed class GetOfficeDocumentOperation : IGetOfficeDocumentOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetOfficeDocumentOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetOfficeDocumentResponse> Execute(string documentId, string url, string accessToken)
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
                RequestUri = new Uri($"{url}/{documentId}")
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
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return new GetOfficeDocumentResponse
                    {
                        ContainsError = true,
                        Error = new ErrorResponse
                        {
                            Code = "internal",
                            Message = ex.Message
                        }
                    };
                }

                return new GetOfficeDocumentResponse
                {
                    ContainsError = true,
                    Error = JsonConvert.DeserializeObject<ErrorResponse>(json)
                };
            }

            return new GetOfficeDocumentResponse
            {
                ContainsError = false,
                OfficeDocument = JsonConvert.DeserializeObject<OfficeDocumentResponse>(json)
            };
        }
    }
}
