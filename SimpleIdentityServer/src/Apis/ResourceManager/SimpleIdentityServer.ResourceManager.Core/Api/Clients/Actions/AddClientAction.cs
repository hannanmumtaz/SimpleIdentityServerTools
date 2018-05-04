﻿using SimpleIdentityServer.Manager.Client;
using SimpleIdentityServer.Manager.Client.DTOs.Responses;
using SimpleIdentityServer.Manager.Common.Responses;
using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.ResourceManager.Core.Models;
using SimpleIdentityServer.ResourceManager.Core.Stores;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Clients.Actions
{
    public interface IAddClientAction
    {
        Task<AddClientResponse> Execute(string subject, ClientResponse request, EndpointTypes type);
    }

    internal sealed class AddClientAction : IAddClientAction
    {
        private readonly IOpenIdManagerClientFactory _openIdManagerClientFactory;
        private readonly IEndpointHelper _endpointHelper;
        private readonly ITokenStore _tokenStore;
        private readonly IRequestHelper _requestHelper;

        public AddClientAction(IOpenIdManagerClientFactory openIdManagerClientFactory,
            IEndpointHelper endpointHelper, ITokenStore tokenStore, IRequestHelper requestHelper)
        {
            _openIdManagerClientFactory = openIdManagerClientFactory;
            _endpointHelper = endpointHelper;
            _tokenStore = tokenStore;
            _requestHelper = requestHelper;
        }

        public async Task<AddClientResponse> Execute(string subject, ClientResponse request, EndpointTypes type)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _requestHelper.UpdateClientResponseTypes(request);
            var endpoint = await _endpointHelper.TryGetEndpointFromProfile(subject, type);
            var grantedToken = await _tokenStore.GetToken(endpoint.AuthUrl, endpoint.ClientId, endpoint.ClientSecret, new[] { Constants.MANAGER_SCOPE });
            return await _openIdManagerClientFactory.GetOpenIdsClient().ResolveAdd(new Uri(endpoint.ManagerUrl), request, grantedToken.AccessToken);
        }
    }
}
