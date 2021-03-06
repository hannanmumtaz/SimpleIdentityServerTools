set packageVersion=%1
echo %packageVersion%

REM COMMON
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Common\SimpleIdentityServer.Module /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Common\SimpleIdentityServer.AccessToken.Store /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Common\SimpleIdentityServer.AccessToken.Store.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Common\SimpleIdentityServer.Logging /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Common\SimpleIdentityServer.Common.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Common\SimpleIdentityServer.Common.Dtos /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM UMA
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.EF /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.EF.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.EF.Postgre /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.EF.Sqlite /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.EF.SqlServer /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Store.Redis /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Store.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Uma\SimpleIdentityServer.Uma.Logging /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM SIMPLEIDSERVER
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.UserManagement /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.UserManagement.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.UserManagement.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Shell /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Authenticate.Basic /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Authenticate.LoginPassword /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Authenticate.SMS /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Authenticate.SMS.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Authenticate.SMS.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Core.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Core.Jwt /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.EF /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.EF.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.EF.Postgre /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.EF.Sqlite /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.EF.SqlServer /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.OpenId.Events /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.OAuth.Events /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.OpenId.Logging /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.OAuth.Logging /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.OAuth2Introspection /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Store /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Store.Redis /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Store.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.TwoFactorAuthentication /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.TwoFactorAuthentication.Email /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.TwoFactorAuthentication.Twilio /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.UserInfoIntrospection /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.Twilio.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.AccountFilter /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.AccountFilter.Basic /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.AccountFilter.Basic.EF /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.AccountFilter.Basic.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.AccountFilter.Basic.EF.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.AccountFilter.Basic.EF.Postgre /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.AccountFilter.Basic.EF.Sqlite /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\SimpleIdServer\SimpleIdentityServer.AccountFilter.Basic.EF.SqlServer /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%

REM SCIM
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Events /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Db.EF /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Db.EF.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Db.EF.Postgre /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Db.EF.Sqlite /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Db.EF.SqlServer /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Host /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Mapping /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Mapping.Ad /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Mapping.Ad.Client /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Mapping.Ad.Common /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Apis\Scim\SimpleIdentityServer.Scim.Mapping.Ad.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%


REM LIB
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Lib\System.Security.Cryptography.Algorithms.Extensions /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Lib\WsFederation /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Lib\Serilog\Serilog.Sinks.ElasticSearch /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Lib\Serilog\Serilog.Sinks.RabbitMQ /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Lib\WebApiContrib\WebApiContrib.Core.Concurrency /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Lib\WebApiContrib\WebApiContrib.Core.Storage /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Lib\WebApiContrib\WebApiContrib.Core.Storage.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Lib\WebApiContrib\WebApiContrib.Core.Storage.Redis /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Lib\Bus\SimpleBus.Core /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%
dotnet pack --output c:\sidfeeds\core\ IdServer\SimpleIdentityServer\src\Lib\Bus\SimpleBus.InMemory /p:PackageVersion=%packageVersion% /p:Version=%packageVersion%