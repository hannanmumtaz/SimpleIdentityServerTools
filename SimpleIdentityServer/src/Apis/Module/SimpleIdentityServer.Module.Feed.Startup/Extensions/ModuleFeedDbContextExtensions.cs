using SimpleIdentityServer.Module.Feed.EF;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SimpleIdentityServer.Module.Feed.Startup.Extensions
{
    internal static class ModuleFeedDbContextExtensions
    {
        public static ModuleFeedDbContext EnsureSeedData(this ModuleFeedDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            AddCategories(dbContext);
            AddProjects(dbContext);
            AddUnits(dbContext);
            AddProjectUnits(dbContext);
            AddConnectors(dbContext);
            AddTwoFactors(dbContext);
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception) { }

            return dbContext;
        }

        private static void AddConnectors(ModuleFeedDbContext dbContext)
        {
            if (!dbContext.Connectors.Any())
            {
                dbContext.Connectors.AddRange(new[]
                {
                    new Connector // Facebook
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        Name = "Facebook",
                        Library = "SimpleIdentityServer.Connectors.Facebook",
                        Version = "3.0.0-rc8",
                        Description = "Refer to this url https://developers.facebook.com to create a new client",
                        Parameters = "ClientId,ClientSecret,Scopes",
                        CreateDateTime = DateTime.UtcNow,
                        UpdateDateTime = DateTime.UtcNow,
                        Picture = "https://blog.addthiscdn.com/wp-content/uploads/2015/11/logo-facebook.png",
                        Scopes = new List<ConnectorScope>(),
                        ConnectorClaims = new List<ConnectorClaim>
                        {
                            new ConnectorClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "id"
                            },
                            new ConnectorClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "age_range",
                                Children = new List<ConnectorClaim>
                                {
                                    new ConnectorClaim
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        Name = "min"
                                    },
                                    new ConnectorClaim
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        Name = "max"
                                    }
                                }
                            },
                            new ConnectorClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "birthday"
                            }
                        },
                        ConnectorClaimRules = new List<ConnectorClaimRule>
                        {
                            new ConnectorClaimRule
                            {
                                Id = Guid.NewGuid().ToString(),
                                Source = "id",
                                Destination = ClaimTypes.NameIdentifier
                            },
                            new ConnectorClaimRule
                            {
                                Id = Guid.NewGuid().ToString(),
                                Source = "birthday",
                                Destination = ClaimTypes.DateOfBirth
                            }
                        },
                        IsSocial = true
                    },
                    new Connector // Microsoft account.
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        Name = "MicrosoftAccount",
                        Library = "SimpleIdentityServer.Connectors.MicrosoftAccount",
                        Version = "3.0.0-rc8",
                        Description = "Refer to this url http://go.microsoft.com/fwlink/?LinkID=144070 to create a new client",
                        Parameters = "ClientId,ClientSecret,Scopes",
                        CreateDateTime = DateTime.UtcNow,
                        UpdateDateTime = DateTime.UtcNow,
                        Picture = "https://is2-ssl.mzstatic.com/image/thumb/Purple128/v4/ad/c7/ce/adc7ce3b-f989-9147-0066-b79383ecc05b/contsched.gvqizhnn.png/1200x630bb.png",
                        Scopes = new List<ConnectorScope>
                        {
                            new ConnectorScope
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "openid",
                                Description = ""
                            },
                            new ConnectorScope
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "email",
                                Description = ""
                            },
                            new ConnectorScope
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "profile",
                                Description = ""
                            },
                            new ConnectorScope
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "offline_access",
                                Description = ""
                            }
                        },
                        IsSocial = true
                    },
                    new Connector // Google
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        Name = "Google",
                        Library = "SimpleIdentityServer.Connectors.Google",
                        Version = "3.0.0-rc8",
                        Description = "Refer to this url https://console.developers.google.com/apis/credentials to create a new client",
                        Parameters = "ClientId,ClientSecret,Scopes",
                        CreateDateTime = DateTime.UtcNow,
                        UpdateDateTime = DateTime.UtcNow,
                        Picture = "https://cdn.icon-icons.com/icons2/1222/PNG/512/1492616990-1-google-search-logo-engine-service-suits_83412.png",
                        Scopes = new List<ConnectorScope>
                        {
                            new ConnectorScope
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "openid"
                            },
                            new ConnectorScope
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "email"
                            },
                            new ConnectorScope
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "profile"
                            }
                        },
                        IsSocial = true
                    },
                    new Connector // Twitter
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        Name = "Twitter",
                        Library = "SimpleIdentityServer.Connectors.Twitter",
                        Version = "3.0.0-rc8",
                        Description = "Refer to this url https://apps.twitter.com/ to create a new client",
                        Parameters = "ClientId,ClientSecret,Scopes",
                        CreateDateTime = DateTime.UtcNow,
                        UpdateDateTime = DateTime.UtcNow,
                        Picture = "https://png.icons8.com/color/1600/twitter-squared.png",
                        IsSocial = true
                    },
                    new Connector // WsFederation
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        Name = "WsFederation",
                        Library = "SimpleIdentityServer.Connectors.WsFederation",
                        Version = "3.0.0-rc8",
                        Description = "Configure WSFederation",
                        Parameters = "",
                        CreateDateTime = DateTime.UtcNow,
                        UpdateDateTime = DateTime.UtcNow,
						Picture = "https://www.midwinter.com.au/wp-content/uploads/2013/04/blue-icon-security.png",
                        IsSocial = false
                    }
                });
            }
        }

        private static void AddTwoFactors(ModuleFeedDbContext dbContext)
        {
            if (!dbContext.TwoFactors.Any())
            {
                dbContext.TwoFactors.AddRange(new[]
                {
                    new TwoFactorAuthenticator
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        Description= "SMS two factor authenticator",
                        Name = "SMS",
                        Picture = "https://png.icons8.com/metro/1600/sms.png",
                        Library = "SimpleIdentityServer.TwoFactorAuthentication.Twilio",
                        Version = "3.0.0-rc8",
                        Parameters = "TwilioAccountSid,TwilioAuthToken,TwilioFromNumber,TwilioMessage",
                        CreateDateTime = DateTime.UtcNow,
                        UpdateDateTime = DateTime.UtcNow
                    }
                });
            }
        }

        private static void AddCategories(ModuleFeedDbContext dbContext)
        {
            if (!dbContext.Categories.Any())
            {
                dbContext.Categories.AddRange(new[]
                {
                    new PackageCategory
                    {
                        Name = "cache"
                    },
                    new PackageCategory
                    {
                        Name = "store"
                    },
                    new PackageCategory
                    {
                        Name = "ui"
                    },
                    new PackageCategory
                    {
                        Name = "handler"
                    },
                    new PackageCategory
                    {
                        Name = "bus"
                    },
                    new PackageCategory
                    {
                        Name = "concurrency"
                    },
                    new PackageCategory
                    {
                        Name = "host"
                    },
                    new PackageCategory
                    {
                        Name = "introspect"
                    },
                    new PackageCategory
                    {
                        Name = "shell"
                    },
                    new PackageCategory
                    {
                        Name = "authenticate"
                    },
                    new PackageCategory
                    {
                        Name = "usermanagement"
                    }
                });
            }
        }

        private static void AddProjects(ModuleFeedDbContext dbContext)
        {
            if (!dbContext.Projects.Any())
            {
                dbContext.Projects.AddRange(new[]
                {
                    new Project
                    {
                        Id = "OpenIdProvider_3.0.0-rc8",
                        Version = "3.0.0-rc8",
                        ProjectName = "OpenIdProvider"
                    },
                    new Project
                    {
                        Id = "EventStore_3.0.0-rc8",
                        Version = "3.0.0-rc8",
                        ProjectName = "EventStore"
                    },
                    new Project
                    {
                        Id = "UmaProvider_3.0.0-rc8",
                        Version = "3.0.0-rc8",
                        ProjectName = "UmaProvider"
                    },
                    new Project
                    {
                        Id = "ScimProvider_3.0.0-rc8",
                        Version = "3.0.0-rc8",
                        ProjectName = "ScimProvider"
                    }
                });
            }
        }

        private static void AddUnits(ModuleFeedDbContext dbContext)
        {
            if (!dbContext.Units.Any())
            {
                dbContext.Units.AddRange(new[]
                {
                    new Unit
                    {
                        UnitName = "authentication",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.OAuth2Introspection",
                                Version = "3.0.0-rc8",
                                CategoryId = "introspect",
                                Parameters = "OauthIntrospectClientId,OauthIntrospectClientSecret,OauthIntrospectAuthUrl"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "concurrency",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "WebApiContrib.Core.Storage.InMemory",
                                Version = "3.0.0-rc8",
                                CategoryId = "cache"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "WebApiContrib.Core.Storage.Redis",
                                Version = "3.0.0-rc8",
                                CategoryId = "cache",
                                Parameters = "RedisCacheInstanceName,RedisCacheConfiguration,RedisCachePort"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "WebApiContrib.Core.Concurrency",
                                Version = "3.0.0-rc8",
                                CategoryId = "concurrency"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "parametersrapi",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Parameter.Host",
                                Version = "3.0.0-rc8",
                                CategoryId = "host"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "accesstokenstore",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.AccessToken.Store.InMemory",
                                Version = "3.0.0-rc8",
                                CategoryId = "store"
                            }
                        }
                    },
                    // OPENID
                    new Unit
                    {
                        UnitName = "openidapi",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Host",
                                Version = "3.0.0-rc8",
                                CategoryId = "host",
                                Parameters = "OpenIdCookieName,OpenIdExternalCookieName,ScimEndpoint,ScimEndpointEnabled"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "oauthstorage",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Store.InMemory",
                                Version = "3.0.0-rc8",
                                CategoryId = "cache"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Store.Redis",
                                Version = "3.0.0-rc8",
                                CategoryId = "cache",
                                Parameters = "OauthRedisStorageConfiguration,OauthRedisStorageInstanceName,OauthRedisStoragePort"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "oauthrepository",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EF.SqlServer",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "OAuthConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EF.Postgre",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "OAuthConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EF.Sqlite",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "OAuthConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EF.InMemory",
                                Version = "3.0.0-rc8",
                                CategoryId = "store"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "openidui",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Shell",
                                Version = "3.0.0-rc8",
                                CategoryId = "shell"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Authenticate.Basic",
                                Version = "3.0.0-rc8",
                                CategoryId = "authenticate",
                                Parameters = "ClientId,ClientSecret,AuthorizationWellKnownConfiguration,BaseScimUrl,IsScimResourceAutomaticallyCreated,ClaimsIncludedInUserCreation"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.UserManagement",
                                Version = "3.0.0-rc8",
                                CategoryId = "usermanagement",
                                Parameters = "CreateScimResourceWhenAccountIsAdded,ClientId,ClientSecret,AuthorizationWellKnownConfiguration,ScimBaseUrl"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "oautheventstore",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.InMemory",
                                Version = "3.0.0-rc8",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Sqlite",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "EventStoreConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Postgre",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "EventStoreConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.SqlServer",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "EventStoreConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Handler",
                                Version = "3.0.0-rc8",
                                CategoryId = "handler",
                                Parameters = "EventStoreHandlerType"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleBus.InMemory",
                                Version = "3.0.0-rc8",
                                CategoryId = "bus"
                            }
                        }
                    },
                    // EVENT STORE
                    new Unit
                    {
                        UnitName = "eventstorehost",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Host",
                                Version = "3.0.0-rc8",
                                CategoryId = "host"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "eventstorerepository",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.InMemory",
                                Version = "3.0.0-rc8",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Postgre",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "EventStoreConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Sqlite",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "EventStoreConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.SqlServer",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "EventStoreConnectionString"
                            }
                        }
                    },
                    // UMA
                    new Unit
                    {
                        UnitName = "umastorage",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Uma.Store.InMemory",
                                Version = "3.0.0-rc8",
                                CategoryId = "cache"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Uma.Store.Redis",
                                Version = "3.0.0-rc8",
                                CategoryId = "cache",
                                Parameters = "UmaRedisStorageConfiguration,UmaRedisStorageInstanceName,UmaRedisStoragePort"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName= "umarepository",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Uma.EF.SqlServer",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "UmaConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Uma.EF.Postgre",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "UmaConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Uma.EF.Sqlite",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "UmaConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Uma.EF.InMemory",
                                Version = "3.0.0-rc8",
                                CategoryId = "store"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "umahost",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Uma.Host",
                                Version = "3.0.0-rc8",
                                CategoryId = "host"
                            }
                        }
                    },
                    // SCIM
                    new Unit
                    {
                        UnitName = "scimhost",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Scim.Host",
                                Version = "3.0.0-rc8",
                                CategoryId = "host"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "scimrepository",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Scim.Db.EF.InMemory",
                                Version = "3.0.0-rc8",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Scim.Db.EF.Postgre",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "ScimConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Scim.Db.EF.Sqlite",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "ScimConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Scim.Db.EF.SqlServer",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "ScimConnectionString"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "scimeventstore",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.InMemory",
                                Version = "3.0.0-rc8",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Sqlite",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "EventStoreConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Postgre",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "EventStoreConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.SqlServer",
                                Version = "3.0.0-rc8",
                                CategoryId = "store",
                                Parameters = "EventStoreConnectionString"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Scim.EventStore.Handler",
                                Version = "3.0.0-rc8",
                                CategoryId = "handler"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleBus.InMemory",
                                Version = "3.0.0-rc8",
                                CategoryId = "bus"
                            }
                        }
                    }
                });
            }
        }

        private static void AddProjectUnits(ModuleFeedDbContext dbContext)
        {
            if (!dbContext.ProjectUnits.Any())
            {
                dbContext.ProjectUnits.AddRange(new[]
                {
                    // OPENID
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        UnitId = "authentication"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        UnitId = "openidapi"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        UnitId = "oauthstorage"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        UnitId = "oauthrepository"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        UnitId = "oautheventstore"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        UnitId = "openidui"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        UnitId = "parametersrapi"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc8",
                        UnitId = "accesstokenstore"
                    },
                    // EVENT STORE.
                    new ProjectUnit
                    {
                        ProjectId = "EventStore_3.0.0-rc8",
                        UnitId = "authentication"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "EventStore_3.0.0-rc8",
                        UnitId = "eventstorehost"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "EventStore_3.0.0-rc8",
                        UnitId = "eventstorerepository"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "EventStore_3.0.0-rc8",
                        UnitId = "parametersrapi"
                    },
                    // UMA
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc8",
                        UnitId = "authentication"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc8",
                        UnitId = "umahost"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc8",
                        UnitId = "oauthstorage"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc8",
                        UnitId = "oauthrepository"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc8",
                        UnitId = "umastorage"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc8",
                        UnitId = "umarepository"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc8",
                        UnitId = "concurrency"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc8",
                        UnitId = "parametersrapi"
                    },
                    // SCIM
                    new ProjectUnit
                    {
                        ProjectId = "ScimProvider_3.0.0-rc8",
                        UnitId = "authentication"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "ScimProvider_3.0.0-rc8",
                        UnitId = "scimhost"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "ScimProvider_3.0.0-rc8",
                        UnitId = "concurrency"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "ScimProvider_3.0.0-rc8",
                        UnitId = "scimeventstore"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "ScimProvider_3.0.0-rc8",
                        UnitId = "scimrepository"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "ScimProvider_3.0.0-rc8",
                        UnitId = "parametersrapi"
                    }
                });
            }
        }
    }
}
