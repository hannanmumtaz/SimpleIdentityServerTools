set packageVersion=%1
echo %packageVersion%

REM PROFILE
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Profile\SimpleIdentityServer.Profile.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Profile\SimpleIdentityServer.Profile.EF /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Profile\SimpleIdentityServer.Profile.EF.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Profile\SimpleIdentityServer.Profile.EF.Postgre /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Profile\SimpleIdentityServer.Profile.EF.Sqlite /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Profile\SimpleIdentityServer.Profile.EF.SqlServer /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Profile\SimpleIdentityServer.Profile.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM MANAGER
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Manager\SimpleIdentityServer.Manager.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Manager\SimpleIdentityServer.Manager.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Manager\SimpleIdentityServer.Manager.Logging /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Manager\SimpleIdentityServer.Manager.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Manager\SimpleIdentityServer.Manager.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM HiERARCHICAL RESOURCE
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\HierarchicalResource\SimpleIdentityServer.HierarchicalResource.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\HierarchicalResource\SimpleIdentityServer.HierarchicalResource.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\HierarchicalResource\SimpleIdentityServer.HierarchicalResource.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\HierarchicalResource\SimpleIdentityServer.HierarchicalResource.EF /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\HierarchicalResource\SimpleIdentityServer.HierarchicalResource.EF.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\HierarchicalResource\SimpleIdentityServer.HierarchicalResource.EF.Postgre /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\HierarchicalResource\SimpleIdentityServer.HierarchicalResource.EF.Sqlite /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\HierarchicalResource\SimpleIdentityServer.HierarchicalResource.EF.SqlServer /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\HierarchicalResource\SimpleIdentityServer.HierarchicalResource.EF.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\HierarchicalResource\SimpleIdentityServer.HierarchicalResource.EF.Resolver /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM EVENT STORE
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.EF /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.Postgre /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.Sqlite /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.SqlServer /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM LICENSE
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\License\SimpleIdentityServer.License /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM CONNECTORS
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.Facebook /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.Google /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.Twitter /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.MicrosoftAccount /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.WsFederation /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM MODULE
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Module\SimpleIdentityServer.Module.Loader /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Module\SimpleIdentityServer.Module.Feed.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Module\SimpleIdentityServer.Module.Feed.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM PARAMETERS
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Parameter\SimpleIdentityServer.Parameter.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Parameter\SimpleIdentityServer.Parameter.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Parameter\SimpleIdentityServer.Parameter.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
REM dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Parameter\SimpleIdentityServer.Parameter.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
