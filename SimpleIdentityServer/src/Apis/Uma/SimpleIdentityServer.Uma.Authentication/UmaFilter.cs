using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Common.DTOs.Responses;
using SimpleIdentityServer.Core.Jwt.Signature;
using SimpleIdentityServer.HierarchicalResource.Client;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Uma.Authentication
{
    public class UmaFilter : IAsyncActionFilter
    {
        private readonly IIdentityServerClientFactory _identityServerClientFactory;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IHierarchicalResourceClientFactory _resourceManagerClientFactory;
        private readonly UmaFilterOptions _options;
        private readonly IDataProtector _dataProtector;
        private readonly IJwsParser _jwsParser;

        public UmaFilter(IIdentityServerClientFactory identityServerClientFactory, IIdentityServerUmaClientFactory identityServerUmaClientFactory,
            IDataProtectionProvider dataProtectionProvider, IHierarchicalResourceClientFactory resourceManagerClientFactory, IJwsParser jwsParser, UmaFilterOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.Authorization == null)
            {
                throw new ArgumentNullException(nameof(options.Authorization));
            }

            if (options.Cookie == null)
            {
                throw new ArgumentNullException(nameof(options.Cookie));
            }

            _options = options;
            _identityServerClientFactory = identityServerClientFactory;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _resourceManagerClientFactory = resourceManagerClientFactory;
            _dataProtector = dataProtectionProvider.CreateProtector("UmaFilter");
            _jwsParser = jwsParser;
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

            var resourceId = string.Empty;
            try
            {
                resourceId = await ResolveResourceId();
            }
            catch(UmaAuthConfigurationException ex)
            {
                context.Result = GetError("internal", ex.Message, HttpStatusCode.InternalServerError);
                return;
            }

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

            var storageValue = GetGrantedToken(context);
            var storedGrantedToken = storageValue == null ? null : storageValue.FirstOrDefault(s => s.ResourceId == resourceId);
            if (storedGrantedToken != null)
            {
                var introspectionResult = await _identityServerClientFactory.CreateAuthSelector()
                    .UseClientSecretPostAuth(_options.Authorization.ClientId, _options.Authorization.ClientSecret)
                    .Introspect(storedGrantedToken.AccessToken, TokenType.AccessToken)
                    .ResolveAsync(_options.Authorization.AuthorizationWellKnownConfiguration);
                if (!introspectionResult.ContainsError)
                {
                    var payload = _jwsParser.GetPayload(storedGrantedToken.AccessToken);
                    if (CheckAccessToken(storedGrantedToken.AccessToken, resourceId))
                    {
                        await next();
                        return;
                    }
                }

                storageValue.Remove(storedGrantedToken);
            }

            var grantedToken = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth(_options.Authorization.ClientId, _options.Authorization.ClientSecret)
                .UseClientCredentials("uma_protection")
                .ResolveAsync(_options.Authorization.AuthorizationWellKnownConfiguration);
            if (grantedToken.ContainsError)
            {
                context.Result = GetError(grantedToken.Error, HttpStatusCode.InternalServerError);
                return;
            }

            var permissionResponse = await _identityServerUmaClientFactory.GetPermissionClient()
                .AddByResolution(new PostPermission
                {
                    ResourceSetId = resourceId,
                    Scopes = Scopes
                }, _options.Authorization.AuthorizationWellKnownConfiguration, grantedToken.Content.AccessToken);
            if (permissionResponse == null || string.IsNullOrWhiteSpace(permissionResponse.TicketId))
            {
                context.Result = GetError("internal", "no_ticket", HttpStatusCode.InternalServerError);
                return;
            }

            var umaGrantedToken = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth(_options.Authorization.ClientId, _options.Authorization.ClientSecret)
                .UseTicketId(permissionResponse.TicketId, identityToken.IdToken)
                .ResolveAsync(_options.Authorization.AuthorizationWellKnownConfiguration);
            if (umaGrantedToken.ContainsError)
            {
                context.Result = GetError(umaGrantedToken.Error, HttpStatusCode.InternalServerError);
                return;
            }

            if (storageValue == null)
            {
                storageValue = new List<EndUserGrantedToken>();
            }

            storageValue.Add(new EndUserGrantedToken
            {
                AccessToken = umaGrantedToken.Content.AccessToken,
                ResourceId = resourceId
            });
            StoreTokens(storageValue, context);
            await next();
        }

        public async Task<string> ResolveResourceId()
        {
            await Task.FromResult(0);
            if (!string.IsNullOrWhiteSpace(ResourceId))
            {
                return ResourceId;
            }

            if (_options.ResourceManager == null || string.IsNullOrWhiteSpace(_options.ResourceManager.ClientId)
                || string.IsNullOrWhiteSpace(_options.ResourceManager.ClientSecret)
                || string.IsNullOrWhiteSpace(_options.ResourceManager.ConfigurationUrl))
            {
                throw new UmaAuthConfigurationException("bad_resourcemanager_config");
            }

            var result = await _resourceManagerClientFactory.GetHierarchicalResourceClient().Get(new Uri(_options.ResourceManager.ConfigurationUrl),
                System.Web.HttpUtility.UrlEncode(ResourceUrl),
                false);
            if (result == null || result.ContainsError || result.Content == null || !result.Content.Any())
            {
                return null;
            }

            return result.Content.First().ResourceId;
        }

        private static IActionResult GetError(ErrorResponseWithState error, HttpStatusCode httpStatusCode)
        {
            return new JsonResult(error)
            {
                StatusCode = (int)httpStatusCode
            };
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

        private ICollection<EndUserGrantedToken> GetGrantedToken(ActionExecutingContext actionExecutingContext)
        {
            if (!actionExecutingContext.HttpContext.Request.Cookies.ContainsKey(_options.Cookie.CookieName))
            {
                return null;
            }

            var cookieValue = actionExecutingContext.HttpContext.Request.Cookies[_options.Cookie.CookieName];
            var unprotected = _dataProtector.Unprotect(cookieValue);
            return JsonConvert.DeserializeObject<ICollection<EndUserGrantedToken>>(unprotected);
        }

        private void StoreTokens(ICollection<EndUserGrantedToken> tokens, ActionExecutingContext actionExecutingContext)
        {
            var now = DateTime.UtcNow;
            var expires = now.AddSeconds(_options.Cookie.ExpiresIn);
            var json = JsonConvert.SerializeObject(tokens);
            var protect = _dataProtector.Protect(json);
            actionExecutingContext.HttpContext.Response.Cookies.Append(_options.Cookie.CookieName, protect, new CookieOptions
            {
                Expires = expires
            });
        }

        private bool CheckAccessToken(string accessToken, string exceptedResourceId)
        {
            var payload = _jwsParser.GetPayload(accessToken);
            if (!payload.ContainsKey("ticket"))
            {
                return false;
            }

            var tickets = JArray.Parse(payload["ticket"].ToString());
            foreach(JObject ticket in tickets)
            {
                if (!ticket.ContainsKey("resource_id"))
                {
                    continue;
                }

                var resourceId = ticket["resource_id"];
                if (exceptedResourceId == resourceId.ToString())
                {
                    return true;
                }
            }

            return false;
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
