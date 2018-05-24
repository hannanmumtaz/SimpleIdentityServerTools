using SimpleIdentityServer.Host.Tests;
using System.Threading.Tasks;
using Xunit;
using SimpleIdentityServer.Common.Client.Factories;
using Moq;
using SimpleIdentityServer.ResourceManager.Client.Configuration;
using System;

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
            Assert.Equal($"{baseUrl}/authpolicies", result.AuthPoliciesEdp);
            Assert.Equal($"{baseUrl}/claims", result.ClaimsEdp);
            Assert.Equal($"{baseUrl}/clients", result.ClientsEdp);
            Assert.Equal($"{baseUrl}/elfinder", result.ElfinderEdp);
            Assert.Equal($"{baseUrl}/endpoints", result.EndpointsEdp);
            Assert.Equal($"{baseUrl}/hierarchicalresources", result.HierarchicalresourcesEdp);
            Assert.Equal($"{baseUrl}/profile", result.ProfileEdp);
            Assert.Equal($"{baseUrl}/resourceowners", result.ResourceOwnersEdp);
            Assert.Equal($"{baseUrl}/resources", result.ResourcesEdp);
            Assert.Equal($"{baseUrl}/scim", result.ScimEdp);
            Assert.Equal($"{baseUrl}/scopes", result.ScopesEdp);
        }

        private void InitializeFakeObjects()
        {
            _httpClientFactoryStub = new Mock<IHttpClientFactory>();
            var getConfigurationOperation = new GetConfigurationOperation(_httpClientFactoryStub.Object);
            _configurationClient = new ConfigurationClient(getConfigurationOperation);
        }
    }
}
