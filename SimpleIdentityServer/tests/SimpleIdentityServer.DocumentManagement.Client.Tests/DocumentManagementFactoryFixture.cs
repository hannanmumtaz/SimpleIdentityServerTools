using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.Tests
{
    public class DocumentManagementFactoryFixture
    {
        public async Task WhenCreateDocumentThenOkIsReturned()
        {
            var identityServerClientFactory = new IdentityServerClientFactory();
            var docMgClientFactory = new DocumentManagementFactory();
            var grantedToken = await identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth("SimpleIdServerClient", "SimpleIdServerClient")
                .UsePassword("administrator", "password", "openid", "profile")
                .ResolveAsync("http://localhost:60000/.well-known/openid-configuration");
            var doc = await docMgClientFactory.GetOfficeDocumentClient().Add(new AddOfficeDocumentRequest
            {
                Id = "docid",
                PrivateKey = "",
                PublicKey = ""
            }, "http://localhost:60010", grantedToken.AccessToken);
        }
    }
}
