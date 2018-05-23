using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Uma.Common.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc.Filters
{
    public class UmaFilter : IAsyncActionFilter
    {
        private const string _authorizationKey = "Authorization";
        private readonly IIdentityServerClientFactory _identityServerClientFactory;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly UmaFilterOptions _options;

        // TODO : Inject IRepresentationManager
        public UmaFilter(IIdentityServerClientFactory identityServerClientFactory, IIdentityServerUmaClientFactory identityServerUmaClientFactory, UmaFilterOptions options)
        {
            _identityServerClientFactory = new IdentityServerClientFactory();
            _identityServerUmaClientFactory = new IdentityServerUmaClientFactory();
            _options = options;
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

            // TODO : TRY TO GET AN EXISTED ACCESS TOKEN FROM THE CACHE.
            var request = context.HttpContext.Request;
            if (!request.Headers.ContainsKey(_authorizationKey))
            {
                context.Result = GetError("authorization", "no_bearer", HttpStatusCode.Forbidden);
                return;
            }

            var authorization = request.Headers[_authorizationKey].First();
            var splittedAuthorization = authorization.Split(' ');
            if (splittedAuthorization.First() != "Bearer" || splittedAuthorization.Count() != 2)
            {
                context.Result = GetError("authorization", "no_bearer", HttpStatusCode.Forbidden);
                return;
            }

            // TODO : TRY TO GET AN ACCESS TOKEN.
            var identityToken = splittedAuthorization.Last();
            var grantedToken = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth(_options.ClientId, _options.ClientSecret)
                .UseClientCredentials("uma_protection")
                .ResolveAsync(_options.AuthorizationWellKnownConfiguration);
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
                }, _options.AuthorizationWellKnownConfiguration, grantedToken.AccessToken);
            if (permissionResponse == null || string.IsNullOrWhiteSpace(permissionResponse.TicketId))
            {
                context.Result = GetError("internal", "no_ticket", HttpStatusCode.InternalServerError);
                return;
            }

            var umaGrantedToken = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth(_options.ClientId, _options.ClientSecret)
                .UseTicketId(permissionResponse.TicketId, identityToken)
                .ResolveAsync(_options.AuthorizationWellKnownConfiguration);
            if (umaGrantedToken == null || string.IsNullOrWhiteSpace(umaGrantedToken.AccessToken))
            {
                context.Result = GetError("authorization", "no_access", HttpStatusCode.Forbidden);
                return;
            }

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
    }
}
