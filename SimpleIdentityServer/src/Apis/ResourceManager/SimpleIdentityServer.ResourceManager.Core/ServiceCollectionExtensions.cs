using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Configuration.Client;
using SimpleIdentityServer.Parameter.Client;
using SimpleIdentityServer.ResourceManager.Core.Api.AuthPolicies;
using SimpleIdentityServer.ResourceManager.Core.Api.AuthPolicies.Actions;
using SimpleIdentityServer.ResourceManager.Core.Api.Claims;
using SimpleIdentityServer.ResourceManager.Core.Api.Claims.Actions;
using SimpleIdentityServer.ResourceManager.Core.Api.Clients;
using SimpleIdentityServer.ResourceManager.Core.Api.Clients.Actions;
using SimpleIdentityServer.ResourceManager.Core.Api.HierarchicalResources;
using SimpleIdentityServer.ResourceManager.Core.Api.HierarchicalResources.Actions;
using SimpleIdentityServer.ResourceManager.Core.Api.Parameters;
using SimpleIdentityServer.ResourceManager.Core.Api.Parameters.Actions;
using SimpleIdentityServer.ResourceManager.Core.Api.Profile;
using SimpleIdentityServer.ResourceManager.Core.Api.Profile.Actions;
using SimpleIdentityServer.ResourceManager.Core.Api.ResourceOwners;
using SimpleIdentityServer.ResourceManager.Core.Api.ResourceOwners.Actions;
using SimpleIdentityServer.ResourceManager.Core.Api.Resources;
using SimpleIdentityServer.ResourceManager.Core.Api.Resources.Actions;
using SimpleIdentityServer.ResourceManager.Core.Api.Scim;
using SimpleIdentityServer.ResourceManager.Core.Api.Scim.Actions;
using SimpleIdentityServer.ResourceManager.Core.Api.Scopes;
using SimpleIdentityServer.ResourceManager.Core.Api.Scopes.Actions;
using SimpleIdentityServer.ResourceManager.Core.Helpers;
using SimpleIdentityServer.ResourceManager.Core.Stores;
using SimpleIdentityServer.Scim.Client;
using SimpleIdentityServer.Uma.Client;
using System;

namespace SimpleIdentityServer.ResourceManager.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddResourceManager(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddIdServerClient();
            services.AddUmaClient();
            services.AddScimClient();
            services.AddOpenIdManagerClient();
            services.AddParameterClient();
            services.AddTransient<IEndpointHelper, EndpointHelper>();
            services.AddTransient<ISearchResourcesetAction, SearchResourcesetAction>();
            services.AddTransient<IResourcesetActions, ResourcesetActions>();
            services.AddTransient<IGetAuthPoliciesByResourceAction, GetAuthPoliciesByResourceAction>();
            services.AddTransient<IGetResourceAction, GetResourceAction>();
            services.AddTransient<IProfileActions, ProfileActions>();
            services.AddTransient<IGetProfileAction, GetProfileAction>();
            services.AddTransient<IUpdateProfileAction, UpdateProfileAction>();
            services.AddTransient<IClientActions, ClientActions>();
            services.AddTransient<IAddClientAction, AddClientAction>();
            services.AddTransient<IDeleteClientAction, DeleteClientAction>();
            services.AddTransient<IGetClientAction, GetClientAction>();
            services.AddTransient<ISearchClientsAction, SearchClientsAction>();
            services.AddTransient<IUpdateClientAction, UpdateClientAction>();
            services.AddTransient<IScopeActions, ScopeActions>();
            services.AddTransient<IAddScopeAction, AddScopeAction>();
            services.AddTransient<IDeleteScopeAction, DeleteScopeAction>();
            services.AddTransient<IGetScopeAction, GetScopeAction>();
            services.AddTransient<ISearchScopesAction, SearchScopesAction>();
            services.AddTransient<IUpdateScopeAction, UpdateScopeAction>();
            services.AddTransient<IRequestHelper, RequestHelper>();
            services.AddTransient<IResourceOwnerActions, ResourceOwnerActions>();
            services.AddTransient<IAddResourceOwnerAction, AddResourceOwnerAction>();
            services.AddTransient<IDeleteResourceOwnerAction, DeleteResourceOwnerAction>();
            services.AddTransient<IGetResourceOwnerAction, GetResourceOwnerAction>();
            services.AddTransient<ISearchResourceOwnersAction, SearchResourceOwnersAction>();
            services.AddTransient<IUpdateResourceOwnerAction, UpdateResourceOwnerAction>();
            services.AddTransient<IClaimActions, ClaimActions>();
            services.AddTransient<IAddClaimAction, AddClaimAction>();
            services.AddTransient<IDeleteClaimAction, DeleteClaimAction>();
            services.AddTransient<IGetClaimAction, GetClaimAction>();
            services.AddTransient<ISearchClaimsAction, SearchClaimsAction>();
            services.AddTransient<IUpdateResourceAction, UpdateResourceAction>();
            services.AddTransient<IDeleteResourceAction, DeleteResourceAction>();
            services.AddTransient<IAddResourceAction, AddResourceAction>();
            services.AddTransient<IAddAuthorizationPolicyAction, AddAuthorizationPolicyAction>();
            services.AddTransient<IDeleteAuthorizationPolicyAction, DeleteAuthorizationPolicyAction>();
            services.AddTransient<IUpdateAuthorizationPolicyAction, UpdateAuthorizationPolicyAction>();
            services.AddTransient<IAuthorizationPolicyActions, AuthorizationPolicyActions>();
            services.AddTransient<IScimActions, ScimActions>();
            services.AddTransient<IGetSchemasAction, GetSchemasAction>();
	        services.AddTransient<ISearchUsersAction, SearchUsersAction>();
	        services.AddTransient<ISearchGroupsAction, SearchGroupsAction>();
            services.AddTransient<IGetUserAction, GetUserAction>();
            services.AddTransient<IGetGroupAction, GetGroupAction>();
            services.AddTransient<IHierarchicalResourcesActions, HierarchicalResourcesActions>();
            services.AddTransient<IAddHierarchicalResourcesAction, AddHierarchicalResourcesAction>();
            services.AddTransient<IDeleteHierarchicalResourcesAction, DeleteHierarchicalResourcesAction>();
            services.AddTransient<IGetHierarchicalResourceAction, GetHierarchicalResourceAction>();
            services.AddTransient<ISearchHierarchicalResourcesAction, SearchHierarchicalResourcesAction>();
            services.AddTransient<IParameterActions, ParameterActions>();
            services.AddTransient<IGetParametersAction, GetParametersAction>();
            services.AddTransient<IUpdateParametersAction, UpdateParametersAction>();
            return services;
        }

        public static IServiceCollection AddInMemoryTokenStore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<ITokenStore, InMemoryTokenStore>();
            return services;
        }
    }
}
