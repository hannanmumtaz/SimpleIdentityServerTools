using SimpleIdentityServer.Core.Common.DTOs.Requests;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.Jwks
{
    public interface IJwksClient
    {
        Task<JsonWebKeySet> ExecuteAsync(Uri jwksUri);
    }

    internal sealed class JwksClient : IJwksClient
    {
        private readonly IGetJwksOperation _getJwksOperation;

        public JwksClient(IGetJwksOperation getJwksOperation)
        {
            _getJwksOperation = getJwksOperation;
        }

        public Task<JsonWebKeySet> ExecuteAsync(Uri jwksUri)
        {
            if (jwksUri == null)
            {
                throw new ArgumentNullException(nameof(jwksUri));
            }

            return _getJwksOperation.ExecuteAsync(jwksUri);
        }
    }
}
