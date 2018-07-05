set ASPNETCORE_ENVIRONMENT=
set DATA_MIGRATED=true
START cmd /k "cd ../IdServer/SimpleIdentityServer/src/Apis/SimpleIdServer/SimpleIdentityServer.Startup && dotnet run -f net461"
START cmd /k "cd ../IdServer/SimpleIdentityServer/src/Apis/Uma/SimpleIdentityServer.Uma.Startup && dotnet run -f net461"
START cmd /k "cd ../IdServer/SimpleIdentityServer/src/Apis/Scim/SimpleIdentityServer.Scim.Startup && dotnet run -f net461"
START cmd /k "cd src/Apis/Manager/SimpleIdentityServer.Manager.Host.Startup && dotnet run -f net461"
START cmd /k "cd src/Apis/Manager/SimpleIdentityServer.Manager.Auth.Host.Startup && dotnet run -f net461"
START cmd /k "cd src/Apis/HierarchicalResource/SimpleIdentityServer.HierarchicalResource.Startup && dotnet run -f net461"
START cmd /k "cd src/Apis/Profile/SimpleIdentityServer.Profile.Startup && dotnet run -f net461"
START cmd /k "cd src/Apis/ResourceManager/SimpleIdentityServer.ResourceManager.Host && dotnet run -f net461"
REM START cmd /k "cd ../IdServer/SimpleIdentityServer/src/Apis/EventStore/SimpleIdentityServer.EventStore.Startup && dotnet run -f net461"
echo Applications are running ...