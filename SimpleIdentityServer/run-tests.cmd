REM MANAGER
dotnet test tests\SimpleIdentityServer.Manager.Core.Tests

REM MODULE FEED
dotnet test tests\SimpleIdentityServer.Module.Feed.Client.Tests
dotnet test tests\SimpleIdentityServer.Module.Feed.Core.Tests
dotnet test tests\SimpleIdentityServer.Module.Loader.Tests