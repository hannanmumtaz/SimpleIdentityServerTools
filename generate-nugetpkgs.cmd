set packageVersion=%1
echo %packageVersion%

REM CONNECTORS
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.Facebook /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.Google /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.Twitter /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.MicrosoftAccount /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Connectors\SimpleIdentityServer.Connectors.WsFederation /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM MODULE
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Module\SimpleIdentityServer.Module.Loader /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Module\SimpleIdentityServer.Module.Feed.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Module\SimpleIdentityServer.Module.Feed.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM PARAMETERS
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Parameter\SimpleIdentityServer.Parameter.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Parameter\SimpleIdentityServer.Parameter.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Parameter\SimpleIdentityServer.Parameter.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Parameter\SimpleIdentityServer.Parameter.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM COMMON
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Common\SimpleIdentityServer.Module /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Common\SimpleIdentityServer.Common.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ SimpleIdentityServer\src\Apis\Common\SimpleIdentityServer.Common.Dtos /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM UMA
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.EF /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.EF.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.EF.Postgre /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.EF.Sqlite /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.EF.SqlServer /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Store.Redis /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Store.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM SIMPLEIDSERVER
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.UserManagement /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Shell /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Authenticate.Basic /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Core.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Core.Jwt /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.EF /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.EF.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.EF.Postgre /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.EF.Sqlite /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.EF.SqlServer /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.EventStore.Handler /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Handler /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Logging /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.OAuth2Introspection /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Store.Redis /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Store.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.TwoFactorAuthentication.Email /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.TwoFactorAuthentication.Twilio /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.UserInfoIntrospection /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM SCIM
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Db.EF /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Db.EF.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Db.EF.Postgre /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Db.EF.Sqlite /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Db.EF.SqlServer /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.EventStore.Handler /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Handler /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM EVENT STORE
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.EF /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.Postgre /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.Sqlite /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Apis\EventStore\SimpleIdentityServer.EventStore.SqlServer /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM LIB
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Lib\System.Security.Cryptography.Algorithms.Extensions /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Lib\WsFederation /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Lib\Serilog\Serilog.Sinks.ElasticSearch /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Lib\Serilog\Serilog.Sinks.RabbitMQ /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Lib\WebApiContrib\WebApiContrib.Core.Concurrency /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Lib\WebApiContrib\WebApiContrib.Core.Storage /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Lib\WebApiContrib\WebApiContrib.Core.Storage.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Lib\WebApiContrib\WebApiContrib.Core.Storage.Redis /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Lib\Bus\SimpleBus.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output d:\sidfeeds\ IdServer\SimpleIdentityServer\src\Lib\Bus\SimpleBus.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%