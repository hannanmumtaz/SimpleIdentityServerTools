using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using WebApiContrib.Core.Storage;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc.Filters
{
    public class UmaFilter : IAsyncActionFilter
    {
        private readonly IIdentityServerClientFactory _identityServerClientFactory;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IStorage _storage;
        private readonly UmaFilterOptions _options;

        public UmaFilter(IIdentityServerClientFactory identityServerClientFactory, IIdentityServerUmaClientFactory identityServerUmaClientFactory,
            IStorage storage, UmaFilterOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.Authorization == null)
            {
                throw new ArgumentNullException(nameof(options.Authorization));
            }

            _options = options;
            if (_options.IdentityTokenFetcher == null)
            {
                _options.IdentityTokenFetcher = new IdTokenCookieFetcher();
            }

            _identityServerClientFactory = new IdentityServerClientFactory();
            _identityServerUmaClientFactory = new IdentityServerUmaClientFactory();
            _storage = storage;
        }
        
        public string ResourceUrl { get; set; }
        public string ResourceId { get; set; }
        public IEnumerable<string> Scopes { get; set; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null)
            {
                await next();
                return;
            }

            var resourceId = await ResolveResourceId();
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                await next();
                return;
            }

            var identityToken = _options.IdentityTokenFetcher.GetIdentityToken(context);
            if (identityToken == null)
            {
                context.Result = GetError("authorization", "no_bearer", HttpStatusCode.Forbidden);
                return;
            }

            var storageKey = $"{identityToken.Subject}";
            var storageValue = await GetGrantedToken(storageKey);
            var storedGrantedToken = storageValue == null ? null : storageValue.FirstOrDefault(s => s.ResourceId == resourceId);
            if (storedGrantedToken != null)
            {
                var introspectionResult = await _identityServerClientFactory.CreateAuthSelector()
                    .UseClientSecretPostAuth(_options.Authorization.ClientId, _options.Authorization.ClientSecret)
                    .Introspect(storedGrantedToken.AccessToken, TokenType.AccessToken)
                    .ResolveAsync(_options.Authorization.AuthorizationWellKnownConfiguration);
                if (introspectionResult.Active)
                {
                    await next();
                    return;
                }

                storageValue.Remove(storedGrantedToken);
            }

            var grantedToken = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth(_options.Authorization.ClientId, _options.Authorization.ClientSecret)
                .UseClientCredentials("uma_protection")
                .ResolveAsync(_options.Authorization.AuthorizationWellKnownConfiguration);
            if (grantedToken == null || string.IsNullOrWhiteSpace(grantedToken.AccessToken))
            {
                context.Result = GetError("internal", "bad_configuration", HttpStatusCode.InternalServerError);
                return;
            }

            var permissionResponse = await _identityServerUmaClientFactory.GetPermissionClient()
                .AddByResolution(new PostPermission
                {
                    ResourceSetId = resourceId,
                    Scopes = Scopes
                }, _options.Authorization.AuthorizationWellKnownConfiguration, grantedToken.AccessToken);
            if (permissionResponse == null || string.IsNullOrWhiteSpace(permissionResponse.TicketId))
            {
                context.Result = GetError("internal", "no_ticket", HttpStatusCode.InternalServerError);
                return;
            }

            var umaGrantedToken = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth(_options.Authorization.ClientId, _options.Authorization.ClientSecret)
                .UseTicketId(permissionResponse.TicketId, identityToken.IdToken)
                .ResolveAsync(_options.Authorization.AuthorizationWellKnownConfiguration);
            if (umaGrantedToken == null || string.IsNullOrWhiteSpace(umaGrantedToken.AccessToken))
            {
                context.Result = GetError("authorization", "no_access", HttpStatusCode.Forbidden);
                return;
            }

            if (storageValue == null)
            {
                storageValue = new List<EndUserGrantedToken>();
            }

            storageValue.Add(new EndUserGrantedToken
            {
                AccessToken = umaGrantedToken.AccessToken,
                ResourceId = resourceId
            });
            await _storage.SetAsync(storageKey, storageValue);
            await next();
        }

        public async Task<string> ResolveResourceId()
        {
            await Task.FromResult(0);
            if (!string.IsNullOrWhiteSpace(ResourceId))
            {
                return ResourceId;
            }

            // TODO : Resolve the resource id.
            return null;
        }

        private static IActionResult GetError(string code, string description, HttpStatusCode httpStatusCode)
        {
            var jObj = new JObject();
            jObj.Add("code", code);
            jObj.Add("message", description);
            return new JsonResult(jObj)
            {
                StatusCode = (int)httpStatusCode
            };
        }

        private async Task<ICollection<EndUserGrantedToken>> GetGrantedToken(string subject)
        {
            return await _storage.TryGetValueAsync<List<EndUserGrantedToken>>(subject).ConfigureAwait(false);
        }

        [DataContract]
        internal class EndUserGrantedToken
        {
            [DataMember(Name = "token")]
            public string AccessToken { get; set; }
            [DataMember(Name = "resource_id")]
            public string ResourceId { get; set; }
        }
    }
}
