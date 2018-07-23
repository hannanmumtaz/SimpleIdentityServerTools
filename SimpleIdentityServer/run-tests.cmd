REM MANAGER
dotnet test tests\SimpleIdentityServer.Manager.Core.Tests
dotnet test tests\SimpleIdentityServer.Manager.Host.Tests

REM MODULE FEED
dotnet test tests\SimpleIdentityServer.Module.Feed.Client.Tests
dotnet test tests\SimpleIdentityServer.Module.Feed.Core.Tests
dotnet test tests\SimpleIdentityServer.Module.Loader.Tests

REM RESOURCE MANAGER
dotnet test tests\SimpleIdentityServer.ResourceManager.API.Host.Tests
dotnet test tests\SimpleIdentityServer.ResourceManager.Core.Tests

REM EVENT STORE
dotnet test tests\SimpleIdentityServer.EventStore.Tests