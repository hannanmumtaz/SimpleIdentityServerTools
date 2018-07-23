using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace SimpleIdentityServer.Eid.UI.Extensions
{
    internal static class ControllerExtensions
    {
        public static IActionResult BuildError(this Controller controller, string code, string message)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            controller.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return new JsonResult(new { code = code, message = message });
        }
    }
}
