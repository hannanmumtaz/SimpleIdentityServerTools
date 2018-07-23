REM MANAGER
dotnet test tests\SimpleIdentityServer.Manager.Core.Tests
dotnet test tests\SimpleIdentityServer.Manager.Host.Tests

REM HIERARCHICAL RESOURE
dotnet test tests\SimpleIdentityServer.HierarchicalResource.Core.Tests
dotnet test tests\SimpleIdentityServer.HierarchicalResource.API.Host.Tests

REM PARAMETER
dotnet test tests\SimpleIdentityServer.Parameter.Core.Tests
dotnet test tests\SimpleIdentityServer.Parameters.Host.Tests

REM MODULE FEED
dotnet test tests\SimpleIdentityServer.Module.Feed.Client.Tests
dotnet test tests\SimpleIdentityServer.Module.Feed.Core.Tests
dotnet test tests\SimpleIdentityServer.Module.Loader.Tests

REM EVENT STORE
dotnet test tests\SimpleIdentityServer.EventStore.Tests