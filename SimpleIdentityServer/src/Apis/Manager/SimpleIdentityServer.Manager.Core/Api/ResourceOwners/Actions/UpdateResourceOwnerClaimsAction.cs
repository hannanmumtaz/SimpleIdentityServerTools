using SimpleIdentityServer.Core.Common.Repositories;
using SimpleIdentityServer.Manager.Core.Errors;
using SimpleIdentityServer.Manager.Core.Exceptions;
using SimpleIdentityServer.Manager.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Manager.Core.Api.ResourceOwners.Actions
{
    public interface IUpdateResourceOwnerClaimsAction
    {
        Task<bool> Execute(UpdateResourceOwnerClaimsParameter request);
    }

    internal class UpdateResourceOwnerClaimsAction : IUpdateResourceOwnerClaimsAction
    {
        private readonly IResourceOwnerRepository _resourceOwnerRepository;
        
        public UpdateResourceOwnerClaimsAction(IResourceOwnerRepository resourceOwnerRepository)
        {
            _resourceOwnerRepository = resourceOwnerRepository;
        }

        public async Task<bool> Execute(UpdateResourceOwnerClaimsParameter request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var resourceOwner = await _resourceOwnerRepository.GetAsync(request.Login);
            if (resourceOwner == null)
            {
                throw new IdentityServerManagerException(ErrorCodes.InvalidParameterCode, string.Format(ErrorDescriptions.TheResourceOwnerDoesntExist, request.Login));
            }

            resourceOwner.UpdateDateTime = DateTime.UtcNow;
            resourceOwner.Claims = request.Claims == null ? new List<Claim>() : request.Claims.Select(c => new Claim(c.Key, c.Value)).ToList();
            Claim updatedClaim, subjectClaim;
            if (((updatedClaim = resourceOwner.Claims.FirstOrDefault(c => c.Type == SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.UpdatedAt)) != null))
            {
                resourceOwner.Claims.Remove(updatedClaim);
            }

            if (((subjectClaim = resourceOwner.Claims.FirstOrDefault(c => c.Type == SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject)) != null))
            {
                resourceOwner.Claims.Remove(subjectClaim);
            }

            resourceOwner.Claims.Add(new Claim(SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject, request.Login));
            resourceOwner.Claims.Add(new Claim(SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.UpdatedAt, DateTime.UtcNow.ToString()));
            var result = await _resourceOwnerRepository.UpdateAsync(resourceOwner);
            if (!result)
            {
                throw new IdentityServerManagerException(ErrorCodes.InternalErrorCode, ErrorDescriptions.TheClaimsCannotBeUpdated);
            }

            return true;
        }
    }
}
