using System.Net.Http;

namespace SimpleIdentityServer.Eid.Ehealth.Factories
{
    public interface IHttpClientFactory
    {
        HttpClient GetHttpClient();
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient GetHttpClient()
        {
            var httpHandler = new HttpClientHandler();
            return new HttpClient(httpHandler);
        }
    }
}
