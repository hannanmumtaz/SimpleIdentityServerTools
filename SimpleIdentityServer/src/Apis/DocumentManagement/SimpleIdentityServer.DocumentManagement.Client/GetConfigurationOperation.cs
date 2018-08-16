using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client
{
    public interface IGetConfigurationOperation
    {
        Task<OfficeDocumentConfigurationResponse> Execute(Uri uri);
    }

    internal class GetConfigurationOperation : IGetConfigurationOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetConfigurationOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<OfficeDocumentConfigurationResponse> Execute(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var serializedContent = await httpClient.GetStringAsync(uri).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<OfficeDocumentConfigurationResponse>(serializedContent);
        }
    }
}