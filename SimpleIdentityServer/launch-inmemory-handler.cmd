set ASPNETCORE_ENVIRONMENT=
set DATA_MIGRATED=true
START cmd /k "cd ../IdServer/SimpleIdentityServer/src/Apis/SimpleIdServer/SimpleIdentityServer.Startup && dotnet run -f net461"
START cmd /k "cd ../IdServer/SimpleIdentityServer/src/Lib/Bus/SimpleBus.Signalr && dotnet run -f net461"
START cmd /k "cd src/Apis/Handlers/SimpleIdentityServer.EventStore.Handler && dotnet run -f net461"
echo Applications are running ...