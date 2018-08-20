set ASPNETCORE_ENVIRONMENT=
set DATA_MIGRATED=true
START cmd /k "cd ../IdServer/SimpleIdentityServer/src/Apis/SimpleIdServer/SimpleIdentityServer.Startup && dotnet run -f net461"
START cmd /k "cd ../IdServer/SimpleIdentityServer/src/Apis/Uma/SimpleIdentityServer.Uma.Startup && dotnet run -f net461"
START cmd /k "cd src/Apis/DocumentManagement/SimpleIdentityServer.DocumentManagement.Startup && dotnet run -f net461"
START cmd /k "cd src/Apis/DocumentManagement/SimpleIdentityServer.DocumentManagement.Website && dotnet run -f net461"