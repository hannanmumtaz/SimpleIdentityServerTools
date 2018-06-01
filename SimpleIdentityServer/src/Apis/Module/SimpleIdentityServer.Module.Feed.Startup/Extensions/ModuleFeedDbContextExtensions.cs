using SimpleIdentityServer.Module.Feed.EF;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;
using System.Linq;

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
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception) { }

            return dbContext;
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
                        Id = "OpenIdProvider_3.0.0-rc7",
                        Version = "3.0.0-rc7",
                        ProjectName = "OpenIdProvider"
                    },
                    new Project
                    {
                        Id = "EventStore_3.0.0-rc7",
                        Version = "3.0.0-rc7",
                        ProjectName = "EventStore"
                    },
                    new Project
                    {
                        Id = "UmaProvider_3.0.0-rc7",
                        Version = "3.0.0-rc7",
                        ProjectName = "UmaProvider"
                    },
                    new Project
                    {
                        Id = "ScimProvider_3.0.0-rc7",
                        Version = "3.0.0-rc7",
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
                        UnitName = "concurrency",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "WebApiContrib.Core.Storage.InMemory",
                                Version = "3.0.0-rc7",
                                CategoryId = "cache"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "WebApiContrib.Core.Storage.Redis",
                                Version = "3.0.0-rc7",
                                CategoryId = "cache"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "WebApiContrib.Core.Concurrency",
                                Version = "3.0.0-rc7",
                                CategoryId = "concurrency"
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
                                Version = "3.0.0-rc7",
                                CategoryId = "cache"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Store.Redis",
                                Version = "3.0.0-rc7",
                                CategoryId = "cache"
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
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EF.Postgre",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EF.Sqlite",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EF.InMemory",
                                Version = "3.0.0-rc7",
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
                                Library = "SimpleIdentityServer.Authenticate.Basic",
                                Version = "3.0.0-rc7",
                                CategoryId = "ui"
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
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Sqlite",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Postgre",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.SqlServer",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Handler",
                                Version = "3.0.0-rc7",
                                CategoryId = "handler"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleBus.InMemory",
                                Version = "3.0.0-rc7",
                                CategoryId = "bus"
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
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Postgre",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Sqlite",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.SqlServer",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "umastorage",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Uma.Store.InMemory",
                                Version = "3.0.0-rc7",
                                CategoryId = "cache"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Uma.Store.Redis",
                                Version = "3.0.0-rc7",
                                CategoryId = "cache"
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
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Uma.EF.Postgre",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Uma.EF.Sqlite",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Uma.EF.InMemory",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
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
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Scim.Db.EF.Postgre",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Scim.Db.EF.Sqlite",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Scim.Db.EF.SqlServer",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
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
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Sqlite",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.Postgre",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.EventStore.SqlServer",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleIdentityServer.Scim.EventStore.Handler",
                                Version = "3.0.0-rc7",
                                CategoryId = "handler"
                            },
                            new UnitPackage
                            {
                                Id = Guid.NewGuid().ToString(),
                                Library = "SimpleBus.InMemory",
                                Version = "3.0.0-rc7",
                                CategoryId = "bus"
                            }
                        }
                    },
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
                        ProjectId = "OpenIdProvider_3.0.0-rc7",
                        UnitId = "oauthstorage"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc7",
                        UnitId = "oauthrepository"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc7",
                        UnitId = "oautheventstore"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc7",
                        UnitId = "openidui"
                    },
                    // EVENT STORE.
                    new ProjectUnit
                    {
                        ProjectId = "EventStore_3.0.0-rc7",
                        UnitId = "eventstorerepository"
                    },
                    // UMA
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc7",
                        UnitId = "oauthstorage"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc7",
                        UnitId = "oauthrepository"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc7",
                        UnitId = "umastorage"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc7",
                        UnitId = "umarepository"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "UmaProvider_3.0.0-rc7",
                        UnitId = "concurrency"
                    },
                    // SCIM
                    new ProjectUnit
                    {
                        ProjectId = "ScimProvider_3.0.0-rc7",
                        UnitId = "concurrency"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "ScimProvider_3.0.0-rc7",
                        UnitId = "scimeventstore"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "ScimProvider_3.0.0-rc7",
                        UnitId = "scimrepository"
                    }
                });
            }
        }
    }
}
