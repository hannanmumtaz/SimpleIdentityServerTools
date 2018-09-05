using SimpleIdentityServer.Core.Common.Models;
using SimpleIdentityServer.Core.Common.Repositories;
using SimpleIdentityServer.Core.Services;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Authenticate.Eid.Services
{
    internal sealed class EidAuthenticateResourceOwnerService : IAuthenticateResourceOwnerService
    {
        private readonly IResourceOwnerRepository _resourceOwnerRepository;
        
        public EidAuthenticateResourceOwnerService(IResourceOwnerRepository resourceOwnerRepository)
        {
            _resourceOwnerRepository = resourceOwnerRepository;
        }

        public string Amr
        {
            get
            {
                return Constants.AMR;
            }
        }

        public async Task<ResourceOwner> AuthenticateResourceOwnerAsync(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            return null;
            /*
            var confirmationCode = await _confirmationCodeStore.Get(password);
            if (confirmationCode == null || confirmationCode.Subject != login)
            {
                return null;
            }

            if (confirmationCode.IssueAt.AddSeconds(confirmationCode.ExpiresIn) <= DateTime.UtcNow)
            {
                return null;
            }

            var resourceOwner = await _resourceOwnerRepository.GetResourceOwnerByClaim(Core.Jwt.Constants.StandardResourceOwnerClaimNames.PhoneNumber, login);
            if (resourceOwner != null)
            {
                await _confirmationCodeStore.Remove(password);
            }

            return resourceOwner;
            */
        }
    }
}
