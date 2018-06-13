using Moq;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Module.Feed.Client.Connectors;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Module.Feed.Client.Tests
{
    public class ConnectorClientFixture : IClassFixture<TestModuleFeedFixture>
    {
        private Mock<IHttpClientFactory> _httpClientFactoryStub;
        private IConnectorClient _connectorClient;
        private readonly TestModuleFeedFixture _server;

        public ConnectorClientFixture(TestModuleFeedFixture server)
        {
            _server = server;
        }

        [Fact]
        public async Task WhenGetAllConnectorsThenListIsReturned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var connectors = await _connectorClient.GetAll("http://localhost:5000/configuration");

            // ASSERT
            Assert.NotNull(connectors);
        }

        [Fact]
        public async Task WhenGetConnectorThenOneRecordIsReturned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var connector = await _connectorClient.Get("name", "http://localhost:5000/configuration");

            // ASSERT
            Assert.NotNull(connector);
            Assert.NotNull(connector.Content);
        }

        private void InitializeFakeObjects()
        {
            _httpClientFactoryStub = new Mock<IHttpClientFactory>();
            var configurationClient = new ConfigurationClient(_httpClientFactoryStub.Object);
            _connectorClient = new ConnectorClient(configurationClient, new GetConnectorOperation(_httpClientFactoryStub.Object),
                new GetAllConnectorsOperation(_httpClientFactoryStub.Object),
                new AddConnectorOperation(_httpClientFactoryStub.Object),
                new DeleteConnectorOperation(_httpClientFactoryStub.Object));
        }
    }
}
