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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Configuration.Core.Repositories;
using SimpleIdentityServer.Configuration.EF.Repositories;
using System;

namespace SimpleIdentityServer.Configuration.EF
{
    public static class SimpleIdentityServerConfigurationEfExtensions
    {
        #region Public static methods

        public static IServiceCollection AddSimpleIdentityServerSqlServer(
            this IServiceCollection services,
            string connectionString)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            RegisterServices(services);
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<SimpleIdentityServerConfigurationContext>(options =>
                    options.UseSqlServer(connectionString));
            return services;
        }

        public static IServiceCollection AddSimpleIdentityServerPostgre(
            this IServiceCollection services,
            string connectionString)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            RegisterServices(services);
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<SimpleIdentityServerConfigurationContext>(options =>
                    options.UseNpgsql(connectionString));
            return services;
        }

        public static IServiceCollection AddSimpleIdentityServerInMemory(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            RegisterServices(services);
            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<SimpleIdentityServerConfigurationContext>(options => options.UseInMemoryDatabase().ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            return services;
        }

        #endregion

        #region Private method

        private static void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAuthenticationProviderRepository, AuthenticationProviderRepository>();
            serviceCollection.AddTransient<ISettingRepository, SettingRepository>();
        }

        #endregion
    }
}
