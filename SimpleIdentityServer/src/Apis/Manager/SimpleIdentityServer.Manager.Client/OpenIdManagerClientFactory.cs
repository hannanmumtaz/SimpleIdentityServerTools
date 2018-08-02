#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Manager.Client.Claims;
using SimpleIdentityServer.Manager.Client.Clients;
using SimpleIdentityServer.Manager.Client.Configuration;
using SimpleIdentityServer.Manager.Client.ResourceOwners;
using SimpleIdentityServer.Manager.Client.Scopes;
using System;

namespace SimpleIdentityServer.Manager.Client
{
    public interface IOpenIdManagerClientFactory
    {
        IConfigurationClient GetConfigurationClient();
        IOpenIdClients GetOpenIdsClient();
        IScopeClient GetScopesClient();
        IResourceOwnerClient GetResourceOwnerClient();
        IClaimsClient GetClaimsClient();
    }

    public sealed class OpenIdManagerClientFactory : IOpenIdManagerClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public OpenIdManagerClientFactory()
        {
            var services = new ServiceCollection();
            RegisterDependencies(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        public OpenIdManagerClientFactory(IHttpClientFactory httpClientFactory)
        {
            var services = new ServiceCollection();
            RegisterDependencies(services, httpClientFactory);
            _serviceProvider = services.BuildServiceProvider();
        }

        public IConfigurationClient GetConfigurationClient()
        {
            var authProviderClient = (IConfigurationClient)_serviceProvider.GetService(typeof(IConfigurationClient));
            return authProviderClient;
        }

        public IOpenIdClients GetOpenIdsClient()
        {
            var configurationClient = (IOpenIdClients)_serviceProvider.GetService(typeof(IOpenIdClients));
            return configurationClient;
        }

        public IResourceOwnerClient GetResourceOwnerClient()
        {
            var resourceOwnerClient = (IResourceOwnerClient)_serviceProvider.GetService(typeof(IResourceOwnerClient));
            return resourceOwnerClient;
        }

        public IScopeClient GetScopesClient()
        {
            var scopesClient = (IScopeClient)_serviceProvider.GetService(typeof(IScopeClient));
            return scopesClient;
        }

        public IClaimsClient GetClaimsClient()
        {
            var claimsClient = (IClaimsClient)_serviceProvider.GetService(typeof(IClaimsClient));
            return claimsClient;
        }

        private static void RegisterDependencies(IServiceCollection serviceCollection, IHttpClientFactory httpClientFactory = null)
        {
            if (httpClientFactory != null)
            {
                serviceCollection.AddSingleton(httpClientFactory);
            }
            else
            {
                serviceCollection.AddCommonClient();
            }

            // Register clients
            serviceCollection.AddTransient<IOpenIdClients, OpenIdClients>();
            serviceCollection.AddTransient<IConfigurationClient, ConfigurationClient>();
            serviceCollection.AddTransient<IScopeClient, ScopeClient>();
            serviceCollection.AddTransient<IResourceOwnerClient, ResourceOwnerClient>();
            serviceCollection.AddTransient<IClaimsClient, ClaimsClient>();

            // Register operations
            serviceCollection.AddTransient<IGetAllClientsOperation, GetAllClientsOperation>();
            serviceCollection.AddTransient<IGetConfigurationOperation, GetConfigurationOperation>();
            serviceCollection.AddTransient<IGetClientOperation, GetClientOperation>();
            serviceCollection.AddTransient<IDeleteClientOperation, DeleteClientOperation>();
            serviceCollection.AddTransient<ISearchClientOperation, SearchClientOperation>();
            serviceCollection.AddTransient<IUpdateClientOperation, UpdateClientOperation>();
            serviceCollection.AddTransient<IAddClientOperation, AddClientOperation>();
            serviceCollection.AddTransient<IAddScopeOperation, AddScopeOperation>();
            serviceCollection.AddTransient<IDeleteScopeOperation, DeleteScopeOperation>();
            serviceCollection.AddTransient<IGetAllScopesOperation, GetAllScopesOperation>();
            serviceCollection.AddTransient<IGetScopeOperation, GetScopeOperation>();
            serviceCollection.AddTransient<IUpdateScopeOperation, UpdateScopeOperation>();
            serviceCollection.AddTransient<IAddResourceOwnerOperation, AddResourceOwnerOperation>();
            serviceCollection.AddTransient<IDeleteResourceOwnerOperation, DeleteResourceOwnerOperation>();
            serviceCollection.AddTransient<IGetAllResourceOwnersOperation, GetAllResourceOwnersOperation>();
            serviceCollection.AddTransient<IGetResourceOwnerOperation, GetResourceOwnerOperation>();
            serviceCollection.AddTransient<IUpdateResourceOwnerClaimsOperation, UpdateResourceOwnerClaimsOperation>();
            serviceCollection.AddTransient<IUpdateResourceOwnerPasswordOperation, UpdateResourceOwnerPasswordOperation>();
            serviceCollection.AddTransient<ISearchScopesOperation, SearchScopesOperation>();
            serviceCollection.AddTransient<ISearchResourceOwnersOperation, SearchResourceOwnersOperation>();
            serviceCollection.AddTransient<IAddClaimOperation, AddClaimOperation>();
            serviceCollection.AddTransient<IDeleteClaimOperation, DeleteClaimOperation>();
            serviceCollection.AddTransient<IGetClaimOperation, GetClaimOperation>();
            serviceCollection.AddTransient<ISearchClaimsOperation, SearchClaimsOperation>();
        }
    }
}
