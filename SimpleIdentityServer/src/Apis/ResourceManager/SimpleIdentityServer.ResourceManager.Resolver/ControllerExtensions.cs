using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.ResourceManager.Resolver
{
    public static class ControllerExtensions
    {
        public static ICollection<string> GetAccessibleResources(this Controller controller, IDataProtector dataProtector)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            if (dataProtector == null)
            {
                throw new ArgumentNullException(nameof(dataProtector));
            }

            if (!controller.Request.Cookies.ContainsKey(Constants.DefaultCookieName))
            {
                return new string[0];
            }

            var cookieValue = controller.Request.Cookies[Constants.DefaultCookieName];
            var unprotected = dataProtector.Unprotect(cookieValue);
            return JsonConvert.DeserializeObject<ICollection<string>>(unprotected);
        }

        public static void PersistAccessibleResources(this Controller controller, 
            ICollection<string> resources, IDataProtector dataProtector, CookieOptions cookieOptions = null)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }

            if (dataProtector == null)
            {
                throw new ArgumentNullException(nameof(dataProtector));
            }

            var now = DateTime.UtcNow;
            var expires = now.AddSeconds(3600);
            cookieOptions = cookieOptions == null ? new CookieOptions
            {
                Expires = expires
            } : cookieOptions;
            var json = JsonConvert.SerializeObject(resources);
            var protect = dataProtector.Protect(json);
            controller.Response.Cookies.Append(Constants.DefaultCookieName, protect, cookieOptions);
        }
    }
}
