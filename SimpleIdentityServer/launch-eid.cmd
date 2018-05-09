START cmd /k "cd src/Apis/Eid/SimpleIdentityServer.Eid.OpenId && dotnet run"
START cmd /k "cd src/Apis/Eid/SimpleIdentityServer.Eid.UI && dotnet electronize start"
START cmd /k "cd src/Apis/RpEid/RpEid.Website && dotnet run -f net461"
START cmd /k "cd src/Apis/RpEid/RpEid.Api && dotnet run -f net461"
echo Applications are running ...