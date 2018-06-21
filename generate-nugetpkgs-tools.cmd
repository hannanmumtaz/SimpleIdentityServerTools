set packageVersion=%1
echo %packageVersion%

REM LICENSE
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\License\SimpleIdentityServer.License /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM CONNECTORS
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.Facebook /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.Google /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.Twitter /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.MicrosoftAccount /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.WsFederation /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM MODULE
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Module\SimpleIdentityServer.Module.Loader /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Module\SimpleIdentityServer.Module.Feed.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Module\SimpleIdentityServer.Module.Feed.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM PARAMETERS
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Parameter\SimpleIdentityServer.Parameter.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Parameter\SimpleIdentityServer.Parameter.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Parameter\SimpleIdentityServer.Parameter.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Parameter\SimpleIdentityServer.Parameter.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM COMMON
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Common\SimpleIdentityServer.Common.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\tools\ SimpleIdentityServer\src\Apis\Common\SimpleIdentityServer.Common.Dtos /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%