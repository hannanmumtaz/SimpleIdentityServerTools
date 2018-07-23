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
using SimpleIdentityServer.Core;
using SimpleIdentityServer.OAuth.Logging;
using SimpleIdentityServer.OpenId.Logging;
using SimpleIdentityServer.Core.Jwt;
using SimpleIdentityServer.Core.Services;
using SimpleIdentityServer.License;
using SimpleIdentityServer.License.Exceptions;
using SimpleIdentityServer.Logging;
using SimpleIdentityServer.Manager.Logging;
using SimpleIdentityServer.Manager.Core;
using System;
using System.Linq;
using SimpleIdentityServer.Manager.Host.Services;

namespace SimpleIdentityServer.Manager.Host.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddSimpleIdentityServerManager(
            this IServiceCollection serviceCollection,
            ManagerOptions managerOptions)
        {
            if (managerOptions == null)
            {
                throw new ArgumentNullException(nameof(managerOptions));
            }

            var loader = new LicenseLoader();
            var license = loader.TryGetLicense();
            if (license.ExpirationDateTime < DateTime.UtcNow)
            {
                throw new LicenseExpiredException();
            }

            // 1. Add the dependencies needed to enable CORS
            serviceCollection.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            // 2. Register all the dependencies.
            serviceCollection.AddSimpleIdentityServerCore();
            serviceCollection.AddSimpleIdentityServerManagerCore();
            // 3. Add authorization policies
            serviceCollection.AddAuthorization(options =>
            {
                options.AddPolicy("manager", policy =>
                {
					policy.AddAuthenticationSchemes("UserInfoIntrospection", "OAuth2Introspection");
                    policy.RequireAssertion(p =>
                    {
                        if (p.User == null || p.User.Identity == null || !p.User.Identity.IsAuthenticated)
                        {
                            return false;
                        }

                        var claimRole = p.User.Claims.FirstOrDefault(c => c.Type == "role");
                        var claimScope = p.User.Claims.FirstOrDefault(c => c.Type == "scope");
                        if (claimRole == null && claimScope == null)
                        {
                            return false;
                        }

                        return claimRole != null && claimRole.Value == "administrator" || claimScope != null && claimScope.Value == "manager";
                    });
                });
            });
            // 5. Add JWT parsers.
            serviceCollection.AddSimpleIdentityServerJwt();
            // 6. Add the dependencies needed to run MVC
			serviceCollection.AddTechnicalLogging();
			serviceCollection.AddManagerLogging();
			serviceCollection.AddOAuthLogging();
			serviceCollection.AddOpenidLogging();
            // TH : REMOVE THIS SERVICE LATER...
            serviceCollection.AddTransient<IPasswordService, DefaultPasswordService>();
        }
    }
}
