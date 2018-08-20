using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments
{
    public interface IDeleteOfficeDocumentConfirmationCodeOperation
    {
        Task<BaseResponse> Execute(string confirmationCode, string url, string accessToken);
    }

    internal sealed class DeleteOfficeDocumentConfirmationCodeOperation : IDeleteOfficeDocumentConfirmationCodeOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteOfficeDocumentConfirmationCodeOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<BaseResponse> Execute(string confirmationCode, string url, string accessToken)
        {
            if(string.IsNullOrWhiteSpace(confirmationCode))
            {
                throw new ArgumentNullException(nameof(confirmationCode));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{url}/{confirmationCode}/invitation")
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
                return new BaseResponse
                {
                    HttpStatus = httpResponse.StatusCode,
                    ContainsError = true,
                    Error = TryGetError(json)
                };
            }

            return new BaseResponse
            {
                ContainsError = false
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
