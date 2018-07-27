param
(
    $config = 'Release'
)

$eventStoreSln = Resolve-Path .\SimpleIdentityServer\SimpleIdentityServer.EventStore.sln
$licenseSln = Resolve-Path .\SimpleIdentityServer\SimpleIdentityServer.License.sln
$managerSln = Resolve-Path .\SimpleIdentityServer\SimpleIdentityServer.Manager.sln
$resourceManagerSln = Resolve-Path .\SimpleIdentityServer\SimpleIdentityServer.ResourceManager.sln
$moduleSln = Resolve-Path .\SimpleIdentityServer\SimpleIdentityServer.Module.sln

dotnet build $eventStoreSln - c $config
dotnet build $licenseSln -c $config
dotnet build $managerSln -c $config
dotnet build $resourceManagerSln -c $config
dotnet build $moduleSln -c $config