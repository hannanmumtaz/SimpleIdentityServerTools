START cmd /k "cd src/Apis/Module/SimpleIdentityServer.Module.Feed.Startup && dotnet run -f net461"
START cmd /k "bash ./wait-for-it.sh localhost:60008 -t 10000 && cd src/Apis/Module/SimpleIdentityServer.OpenId.Modularized.Startup && dotnet run -f net461"
START cmd /k "bash ./wait-for-it.sh localhost:60000 -t 10000 && cd src/Apis/Module/SimpleIdentityServer.ScimProvider.Modularized.Startup && dotnet run -f net461"
START cmd /k "bash ./wait-for-it.sh localhost:60001 -t 10000 && cd src/Apis/Module/SimpleIdentityServer.Uma.Modularized.Startup && dotnet run -f net461"
START cmd /k "bash ./wait-for-it.sh localhost:60004 -t 10000 && cd src/Apis/Module/SimpleIdentityServer.EventStore.Modularized.Startup && dotnet run -f net461"
START cmd /k "cd src/Apis/Manager/SimpleIdentityServer.Manager.Host.Startup && dotnet run -f net461"
START cmd /k "cd src/Apis/Manager/SimpleIdentityServer.Manager.Auth.Host.Startup && dotnet run -f net461"
START cmd /k "cd src/Apis/ResourceManager/SimpleIdentityServer.ResourceManager.API.Startup && dotnet run -f net461"
START cmd /k "cd src/Apis/ResourceManager/SimpleIdentityServer.ResourceManager.Host&& dotnet run -f net461"