using Microsoft.AspNetCore.Builder;
using System;

namespace SimpleIdentityServer.DocumentManagement.Api.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDocumentManagementExceptionHandler(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
            {
                throw new ArgumentNullException(nameof(applicationBuilder));
            }

            return applicationBuilder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
