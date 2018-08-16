namespace SimpleIdentityServer.DocumentManagement.Client.Tests
{
    public class SharedContext
    {
        public SharedContext()
        {
            HttpClientFactory = new FakeHttpClientFactory();
        }

        public FakeHttpClientFactory HttpClientFactory { get; }
    }
}
