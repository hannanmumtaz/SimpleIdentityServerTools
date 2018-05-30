using Microsoft.AspNetCore.Http;

namespace SimpleIdentityServer.Module.Feed.Host.Extensions
{
    internal static class HttpRequestsExtensions
    {
        public static string GetAbsoluteUriWithVirtualPath(this HttpRequest requestMessage)
        {
            var host = requestMessage.Host.Value;
            var http = "http://";
            if (requestMessage.IsHttps)
            {
                http = "https://";
            }

            var relativePath = requestMessage.PathBase.Value;
            return http + host + relativePath;
        }
    }
}
