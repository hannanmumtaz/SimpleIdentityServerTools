using Moq;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.DocumentManagement.Client.Jwks;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.DocumentManagement.Client.Tests.Clients
{
    public class JwksClientFixture : IClassFixture<TestDocumentManagementServerFixture>
    {
        private const string baseUrl = "http://localhost:5000";
        private readonly TestDocumentManagementServerFixture _server;
        private Mock<IHttpClientFactory> _httpClientFactoryStub;
        private Jwks.IJwksClient _jwksClient;

        public JwksClientFixture(TestDocumentManagementServerFixture server)
        {
            _server = server;
        }

        [Fact]
        public async Task When_Get_Jwks_Then_List_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT 
            var jwks = await _jwksClient.ResolveAsync(baseUrl + "/configuration");

            // ASSERT
            Assert.NotNull(jwks);
            Assert.Equal(1, jwks.Keys.Count());
        }

        private void InitializeFakeObjects()
        {
            _httpClientFactoryStub = new Mock<IHttpClientFactory>();
            var getJsonWebKeysOperation = new GetJwksOperation(_httpClientFactoryStub.Object);
            var getConfigurationOperation = new GetConfigurationOperation(_httpClientFactoryStub.Object);
            _jwksClient = new JwksClient(getJsonWebKeysOperation, getConfigurationOperation);
        }
    }
}
