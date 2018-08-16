using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Core.Common.DTOs.Requests;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.Jwks
{
    public interface IGetJwksOperation
    {
        Task<JsonWebKeySet> ExecuteAsync(Uri jwksUri);
    }

    internal sealed class GetJwksOperation : IGetJwksOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetJwksOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<JsonWebKeySet> ExecuteAsync(Uri jwksUri)
        {
            if (jwksUri == null)
            {
                throw new ArgumentNullException(nameof(jwksUri));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var serializedContent = await httpClient.GetStringAsync(jwksUri).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<JsonWebKeySet>(serializedContent);
        }
    }
}
