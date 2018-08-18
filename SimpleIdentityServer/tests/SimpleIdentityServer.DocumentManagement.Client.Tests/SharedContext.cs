using Moq;
using SimpleIdentityServer.AccessToken.Store;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Store;

namespace SimpleIdentityServer.DocumentManagement.Client.Tests
{
    public class SharedContext
    {
        public SharedContext()
        {
            HttpClientFactory = new FakeHttpClientFactory();
            AccessTokenStore = new Mock<IAccessTokenStore>();
            IdentityServerUmaClientFactory = new Mock<IIdentityServerUmaClientFactory>();
        }

        public FakeHttpClientFactory HttpClientFactory { get; }
        public Mock<IAccessTokenStore> AccessTokenStore { get; }
        public Mock<IIdentityServerUmaClientFactory> IdentityServerUmaClientFactory { get; }
        public IOfficeDocumentConfirmationLinkStore OfficeDocumentConfirmationLinkStore;
    }
}
