REM LAUNCH FEED
START cmd /k "cd src/Apis/Module/SimpleIdentityServer.Module.Feed.Startup && dotnet run -f net461"

REM LAUNCH MODULARIZED OPENID
START cmd /k "cd src/Apis/Module/SimpleIdentityServer.OpenId.Modularized.Startup && dotnet run -f net461"

REM LAUNCH MODULARIZED UMA
START cmd /k "cd src/Apis/Module/SimpleIdentityServer.ScimProvider.Modularized.Startup && dotnet run -f net461"

REM LAUNCH MODULARIZED SCIM
START cmd /k "cd src/Apis/Module/SimpleIdentityServer.Uma.Modularized.Startup && dotnet run -f net461"

REM LAUNCH EVENT STORE
START cmd /k "cd src/Apis/Module/SimpleIdentityServer.EventStore.Modularized.Startup && dotnet run -f net461"