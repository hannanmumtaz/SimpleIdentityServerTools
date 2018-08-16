using Microsoft.AspNetCore.Builder;
using SimpleIdentityServer.DocumentManagement.Api.Middlewares;
using System;

namespace SimpleIdentityServer.DocumentManagement.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDocumentManagementApi(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseDocumentManagementExceptionHandler();
            return app;
        }
    }
}
