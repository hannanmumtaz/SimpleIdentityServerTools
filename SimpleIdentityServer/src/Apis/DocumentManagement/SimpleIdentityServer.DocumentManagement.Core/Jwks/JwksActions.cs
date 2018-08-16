using SimpleIdentityServer.Core.Common.DTOs.Requests;
using SimpleIdentityServer.DocumentManagement.Core.Jwks.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.Jwks
{
    public interface IJwksActions
    {
        Task<JsonWebKeySet> GetJwks();
    }

    internal sealed class JwksActions : IJwksActions
    {
        private readonly IGetJwksAction _getJwksAction;

        public JwksActions(IGetJwksAction getJwksAction)
        {
            _getJwksAction = getJwksAction;
        }

        public async Task<JsonWebKeySet> GetJwks()
        {
            var enc = await _getJwksAction.Execute();
            var result = new JsonWebKeySet
            {
                Keys = new List<Dictionary<string, object>>()
            };

            result.Keys.AddRange(enc);
            return result;
        }
    }
}
