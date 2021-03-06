﻿#region copyright
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
using SimpleIdentityServer.Core.Api.Registration.Actions;
using SimpleIdentityServer.Core.WebSite.User.Actions;
using SimpleIdentityServer.Manager.Core.Api.Claims;
using SimpleIdentityServer.Manager.Core.Api.Claims.Actions;
using SimpleIdentityServer.Manager.Core.Api.Clients;
using SimpleIdentityServer.Manager.Core.Api.Clients.Actions;
using SimpleIdentityServer.Manager.Core.Api.Jwe;
using SimpleIdentityServer.Manager.Core.Api.Jwe.Actions;
using SimpleIdentityServer.Manager.Core.Api.Jws;
using SimpleIdentityServer.Manager.Core.Api.Jws.Actions;
using SimpleIdentityServer.Manager.Core.Api.Manage.Actions;
using SimpleIdentityServer.Manager.Core.Api.ResourceOwners;
using SimpleIdentityServer.Manager.Core.Api.ResourceOwners.Actions;
using SimpleIdentityServer.Manager.Core.Api.Scopes;
using SimpleIdentityServer.Manager.Core.Api.Scopes.Actions;
using SimpleIdentityServer.Manager.Core.Factories;
using SimpleIdentityServer.Manager.Core.Helpers;
using SimpleIdentityServer.Manager.Core.Manage;
using SimpleIdentityServer.Manager.Core.Validators;

namespace SimpleIdentityServer.Manager.Core
{
    public static class AddSimpleIdentityServerManagerCoreExtensions
    {
        public static IServiceCollection AddSimpleIdentityServerManagerCore(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IJwsActions, JwsActions>();
            serviceCollection.AddTransient<IGetJwsInformationAction, GetJwsInformationAction>();
            serviceCollection.AddTransient<IJweActions, JweActions>();
            serviceCollection.AddTransient<IGetJweInformationAction, GetJweInformationAction>();
            serviceCollection.AddTransient<ICreateJweAction, CreateJweAction>();
            serviceCollection.AddTransient<ICreateJwsAction, CreateJwsAction>();
            serviceCollection.AddTransient<IJsonWebKeyHelper, JsonWebKeyHelper>();
            serviceCollection.AddTransient<IHttpClientFactory, HttpClientFactory>();
            serviceCollection.AddTransient<IJsonWebKeyEnricher, JsonWebKeyEnricher>();
            serviceCollection.AddTransient<IClientActions, ClientActions>();
            serviceCollection.AddTransient<IGetClientsAction, GetClientsAction>();
            serviceCollection.AddTransient<IGetClientAction, GetClientAction>();
            serviceCollection.AddTransient<IRemoveClientAction, RemoveClientAction>();
            serviceCollection.AddTransient<ISearchClientsAction, SearchClientsAction>();
            serviceCollection.AddTransient<IUpdateClientAction, UpdateClientAction>();
            serviceCollection.AddTransient<IScopeActions, ScopeActions>();
            serviceCollection.AddTransient<IDeleteScopeOperation, DeleteScopeOperation>();
            serviceCollection.AddTransient<IGetScopeOperation, GetScopeOperation>();
            serviceCollection.AddTransient<IGetScopesOperation, GetScopesOperation>();
            serviceCollection.AddTransient<IGetResourceOwnersAction, GetResourceOwnersAction>();
            serviceCollection.AddTransient<IGetResourceOwnerAction, GetResourceOwnerAction>();
            serviceCollection.AddTransient<IUpdateResourceOwnerClaimsAction, UpdateResourceOwnerClaimsAction>();
            serviceCollection.AddTransient<IUpdateResourceOwnerPasswordAction, UpdateResourceOwnerPasswordAction>();
            serviceCollection.AddTransient<IUpdateResourceOwnerClaimsParameterValidator, UpdateResourceOwnerClaimsParameterValidator>();
            serviceCollection.AddTransient<IUpdateResourceOwnerPasswordParameterValidator, UpdateResourceOwnerPasswordParameterValidator>();
            serviceCollection.AddTransient<IAddUserParameterValidator, AddUserParameterValidator>();
            serviceCollection.AddTransient<IResourceOwnerActions, ResourceOwnerActions>();
            serviceCollection.AddTransient<IDeleteResourceOwnerAction, DeleteResourceOwnerAction>();
            serviceCollection.AddTransient<IRegisterClientAction, RegisterClientAction>();
            serviceCollection.AddTransient<IAddUserOperation, AddUserOperation>();
            serviceCollection.AddTransient<IAddScopeOperation, AddScopeOperation>();
            serviceCollection.AddTransient<IUpdateScopeOperation, UpdateScopeOperation>();
            serviceCollection.AddTransient<IManageActions, ManageActions>();
            serviceCollection.AddTransient<IExportAction, ExportAction>();
            serviceCollection.AddTransient<ISearchScopesOperation, SearchScopesOperation>();
            serviceCollection.AddTransient<IImportAction, ImportAction>();
            serviceCollection.AddTransient<ISearchResourceOwnersAction, SearchResourceOwnersAction>();
            serviceCollection.AddTransient<IClaimActions, ClaimActions>();
            serviceCollection.AddTransient<IAddClaimAction, AddClaimAction>();
            serviceCollection.AddTransient<IDeleteClaimAction, DeleteClaimAction>();
            serviceCollection.AddTransient<IGetClaimAction, GetClaimAction>();
            serviceCollection.AddTransient<ISearchClaimsAction, SearchClaimsAction>();
            serviceCollection.AddTransient<IGetClaimsAction, GetClaimsAction>();
            return serviceCollection;
        }
    }
}
