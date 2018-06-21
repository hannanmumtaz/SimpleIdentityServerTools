param
(
    $config = 'Release'
)

$managerSln = Resolve-Path .\SimpleIdentityServer\SimpleIdentityServer.Manager.sln
$resourceManagerSln = Resolve-Path .\SimpleIdentityServer\SimpleIdentityServer.ResourceManager.sln
$licenseSln = Resolve-Path .\SimpleIdentityServer\SimpleIdentityServer.License.sln

dotnet build $managerSln -c $config
dotnet build $resourceManagerSln -c $config
dotnet build $licenseSln -c $config