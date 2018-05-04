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

using System;
using SimpleIdentityServer.Configuration.Client.AuthProvider;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Configuration.Client.Factory;
using SimpleIdentityServer.Configuration.Client.Setting;
using SimpleIdentityServer.Configuration.Client.Representation;

namespace SimpleIdentityServer.Configuration.Client
{
    public interface ISimpleIdServerConfigurationClientFactory
    {
        IAuthProviderClient GetAuthProviderClient();
        ISettingClient GetSettingClient();
        IRepresentationClient GetRepresentationClient();
    }

    public class SimpleIdServerConfigurationClientFactory : ISimpleIdServerConfigurationClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        #region Constructor

        public SimpleIdServerConfigurationClientFactory()
        {
            var services = new ServiceCollection();
            RegisterDependencies(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        #endregion

        #region Public methods

        public IAuthProviderClient GetAuthProviderClient()
        {
            var authProviderClient = (IAuthProviderClient)_serviceProvider.GetService(typeof(IAuthProviderClient));
            return authProviderClient;
        }

        public IGetConfigurationOperation GetConfiguration()
        {
            var configurationClient = (IGetConfigurationOperation)_serviceProvider.GetService(typeof(IGetConfigurationOperation));
            return configurationClient;
        }

        public ISettingClient GetSettingClient()
        {
            var settingClient = (ISettingClient)_serviceProvider.GetService(typeof(ISettingClient));
            return settingClient;
        }

        public IRepresentationClient GetRepresentationClient()
        {
            var representationClient = (IRepresentationClient)_serviceProvider.GetService(typeof(IRepresentationClient));
            return representationClient;
        }

        #endregion

        #region Private static methods

        private static void RegisterDependencies(IServiceCollection serviceCollection)
        {
            // Register clients
            serviceCollection.AddTransient<IAuthProviderClient, AuthProviderClient>();
            serviceCollection.AddTransient<ISettingClient, SettingClient>();
            serviceCollection.AddTransient<IRepresentationClient, RepresentationClient>();
            
            // Regsiter factories
            serviceCollection.AddTransient<IHttpClientFactory, HttpClientFactory>();

            // Register operations
            serviceCollection.AddTransient<IDisableAuthProviderOperation, DisableAuthProviderOperation>();
            serviceCollection.AddTransient<IEnableAuthProviderOperation, EnableAuthProviderOperation>();
            serviceCollection.AddTransient<IGetAuthProviderOperation, GetAuthProviderOperation>();
            serviceCollection.AddTransient<IGetAuthProvidersOperation, GetAuthProvidersOperation>();
            serviceCollection.AddTransient<IGetConfigurationOperation, GetConfigurationOperation>();
            serviceCollection.AddTransient<IDeleteSettingOperation, DeleteSettingOperation>();
            serviceCollection.AddTransient<IGetSettingOperation, GetSettingOperation>();
            serviceCollection.AddTransient<IGetSettingsOperation, GetSettingsOperation>();
            serviceCollection.AddTransient<IUpdateSettingOperation, UpdateSettingOperation>();
            serviceCollection.AddTransient<IDeleteRepresentationsOperation, DeleteRepresentationsOperation>();
            serviceCollection.AddTransient<IGetRepresentationsOperation, GetRepresentationsOperation>();
        }

        #endregion
    }
}
