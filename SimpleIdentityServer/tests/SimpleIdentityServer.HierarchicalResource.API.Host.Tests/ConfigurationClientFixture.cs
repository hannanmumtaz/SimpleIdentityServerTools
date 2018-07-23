using SimpleIdentityServer.Host.Tests;
using System.Threading.Tasks;
using Xunit;
using Moq;
using System;
using SimpleIdentityServer.HierarchicalResource.Client.Configuration;
using SimpleIdentityServer.Common.Client.Factories;

namespace SimpleIdentityServer.ResourceManager.API.Host.Tests
{
    public class ConfigurationClientFixture : IClassFixture<TestResourceManagerFixture>
    {
        private const string baseUrl = "http://localhost:5000";
        private readonly TestResourceManagerFixture _server;
        private Mock<IHttpClientFactory> _httpClientFactoryStub;
        private IConfigurationClient _configurationClient;

        public ConfigurationClientFixture(TestResourceManagerFixture server)
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
            var result = await _configurationClient.GetConfiguration(new Uri($"{baseUrl}/configuration"));

            // ASSERTS
            Assert.NotNull(result);
            Assert.Equal($"{baseUrl}/hierarchicalresources", result.HierarchicalresourcesEdp);
        }

        private void InitializeFakeObjects()
        {
            _httpClientFactoryStub = new Mock<IHttpClientFactory>();
            var getConfigurationOperation = new GetConfigurationOperation(_httpClientFactoryStub.Object);
            _configurationClient = new ConfigurationClient(getConfigurationOperation);
        }
    }
}
