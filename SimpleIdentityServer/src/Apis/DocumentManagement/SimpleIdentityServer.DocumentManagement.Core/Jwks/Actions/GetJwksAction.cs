using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.Jwks.Actions
{
    public interface IGetJwksAction
    {
        Task<List<Dictionary<string, object>>> Execute();
    }

    internal sealed class GetJwksAction : IGetJwksAction
    {
        private readonly IJsonWebKeyRepository _jsonWebKeyRepository;
        private readonly IJsonWebKeyEnricher _jsonWebKeyEnricher;

        public GetJwksAction(IJsonWebKeyRepository jsonWebKeyRepository, IJsonWebKeyEnricher jsonWebKeyEnricher)
        {
            _jsonWebKeyRepository = jsonWebKeyRepository;
            _jsonWebKeyEnricher = jsonWebKeyEnricher;
        }

        public async Task<List<Dictionary<string, object>>> Execute()
        {
            var result = new List<Dictionary<string, object>>();
            var jsonWebKeys = await _jsonWebKeyRepository.GetAllAsync();
            foreach (var jsonWebKey in jsonWebKeys)
            {
                var publicKeyInformation = _jsonWebKeyEnricher.GetPublicKeyInformation(jsonWebKey);
                var jsonWebKeyInformation = _jsonWebKeyEnricher.GetJsonWebKeyInformation(jsonWebKey);
                foreach (var jwk in jsonWebKeyInformation)
                {
                    publicKeyInformation.Add(jwk.Key, jwk.Value);
                }

                result.Add(publicKeyInformation);
            }

            return result;
        }
    }
}