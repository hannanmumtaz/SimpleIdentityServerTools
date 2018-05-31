REM LAUNCH FEED
START cmd /k "cd src/Apis/Module/SimpleIdentityServer.Module.Feed.Startup && dotnet run -f net461"

REM LAUNCH MODULARIZED OPENID
START cmd /k "cd src/Apis/Module/SimpleIdentityServer.OpenId.Modularized.Startup && dotnet run -f net461"

REM LAUNCH MODULARIZED UMA

REM LAUNCH MODULARIZED SCIM