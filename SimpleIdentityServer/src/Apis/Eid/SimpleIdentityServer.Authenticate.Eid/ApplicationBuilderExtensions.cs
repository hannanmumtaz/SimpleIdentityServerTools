using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using SimpleIdentityServer.Authenticate.Eid.Controllers;
using System;

namespace SimpleIdentityServer.Authenticate.Eid
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseEidStaticFiles(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var assembly = typeof(AuthenticateController).Assembly;
            var embeddedFileProvider = new EmbeddedFileProvider(assembly, "SimpleIdentityServer.Authenticate.Eid.wwwroot");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = embeddedFileProvider
            });
            return app;
        }
    }
}
