using Microsoft.AspNetCore.Http;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Core.Common.Extensions;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var apiException = exception as BaseDocumentManagementApiException;
                if (apiException == null)
                {
                    apiException = new BaseDocumentManagementApiException("unhandled_exception", exception.Message);
                }

                context.Response.Clear();
                var error = new ErrorResponse
                {
                    Error = apiException.Code,
                    ErrorDescription = apiException.Message
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                var serializedError = error.SerializeWithDataContract();
                await context.Response.WriteAsync(serializedError);
            }
        }
    }
}
