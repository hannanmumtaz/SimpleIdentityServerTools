﻿using SimpleIdentityServer.Profile.Core.Models;
using SimpleIdentityServer.Profile.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Profile.Core.Api.Profile.Actions
{
    public interface IGetProfileAction
    {
        Task<ProfileAggregate> Execute(string subject);
    }

    internal sealed class GetProfileAction : IGetProfileAction
    {
        private readonly IProfileRepository _profileRepository;

        public GetProfileAction(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public Task<ProfileAggregate> Execute(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            return _profileRepository.Get(subject);
        }
    }
}
