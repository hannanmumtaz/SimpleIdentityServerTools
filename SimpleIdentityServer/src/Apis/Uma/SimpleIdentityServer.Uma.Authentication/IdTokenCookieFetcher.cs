using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;

namespace SimpleIdentityServer.Uma.Authentication
{
    public interface IIdentityTokenFetcher
    {
        IdentityTokenFetcherResult GetIdentityToken(ActionExecutingContext context);
    }

    public class IdentityTokenFetcherResult
    {
        public string IdToken { get; set; }
        public string Subject { get; set; }
    }

    internal class IdTokenCookieFetcher : IIdentityTokenFetcher
    {
        public IdentityTokenFetcherResult GetIdentityToken(ActionExecutingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.HttpContext.User == null || context.HttpContext.User.Identity == null || !context.HttpContext.User.Identity.IsAuthenticated || !(context.HttpContext.User.Identity is ClaimsIdentity))
            {
                return null;
            }

            var idClaims = (ClaimsIdentity)context.HttpContext.User.Identity;
            if (idClaims.Claims == null)
            {
                return null;
            }

            var idTokenClaim = idClaims.Claims.FirstOrDefault(c => c.Type == "id_token");
            var subjectClaim = idClaims.Claims.FirstOrDefault(c => c.Type == "sub");
            if (idTokenClaim == null || subjectClaim == null)
            {
                return null;
            }

            return new IdentityTokenFetcherResult
            {
                IdToken = idTokenClaim.Value,
                Subject = subjectClaim.Value
            };
        }
    }
}
