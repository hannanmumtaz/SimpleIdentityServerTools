using Moq;
using SimpleIdentityServer.Common.Client.Factories;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Module.Feed.Client.Tests
{
    public class ConfigurationClientFixture : IClassFixture<TestModuleFeedFixture>
    {
        private Mock<IHttpClientFactory> _httpClientFactoryStub;
        private IConfigurationClient _configurationClient;
        private readonly TestModuleFeedFixture _server;

        public ConfigurationClientFixture(TestModuleFeedFixture server)
        {
            _server = server;
        }

        [Fact]
        public async Task WhenGetConfigurationThenEndpointsAreReturned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var result = await _configurationClient.GetConfiguration("http://localhost:5000/configuration").ConfigureAwait(false);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal("http://localhost:5000/projects", result.ProjectsEndpoint);
        }

        private void InitializeFakeObjects()
        {
            _httpClientFactoryStub = new Mock<IHttpClientFactory>();
            _configurationClient = new ConfigurationClient(_httpClientFactoryStub.Object);
        }
    }
}
