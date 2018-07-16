set ASPNETCORE_ENVIRONMENT=
set DATA_MIGRATED=true
START cmd /k "cd ../IdServer/SimpleIdentityServer/src/Apis/SimpleIdServer/SimpleIdentityServer.Startup && dotnet run -f net461"
START cmd /k "cd src/Apis/ResourceManager/SimpleIdentityServer.ResourceManager.Host && dotnet run -f net461"
START cmd /k "cd src/Apis/ResourceManager/SimpleIdentityServer.Merged.API.Startup && dotnet run -f net461"
REM START cmd /k "cd ../IdServer/SimpleIdentityServer/src/Apis/EventStore/SimpleIdentityServer.EventStore.Startup && dotnet run -f net461"
echo Applications are running ...