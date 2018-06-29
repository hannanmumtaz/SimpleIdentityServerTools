using System;
using System.Linq;
using System.Security.Claims;

namespace SimpleIdentityServer.Profile.Host.Extensions
{
    internal static class ClaimsPrincipalExtension
    {
        public static string GetSubject(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException(nameof(claimsPrincipal));
            }

            var subjectClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "sub");
            if (subjectClaim == null)
            {
                return null;
            }

            return subjectClaim.Value;
        }
    }
}