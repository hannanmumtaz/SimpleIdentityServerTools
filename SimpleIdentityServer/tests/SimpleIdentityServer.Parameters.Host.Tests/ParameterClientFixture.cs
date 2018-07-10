using Moq;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Parameter.Client;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Parameters.Host.Tests
{
    public class ParameterClientFixture : IClassFixture<ParameterServerFixture>
    {
        private const string baseUrl = "http://localhost:5000";
        private readonly ParameterServerFixture _server;
        private Mock<IHttpClientFactory> _httpClientFactoryStub;
        private IGetModulesAction _getModulesAction;
        private IUpdateModulesAction _updateModuleAction;

        public ParameterClientFixture(ParameterServerFixture server)
        {
            _server = server;
        }

        [Fact]
        public async Task WhenGetModulesThenResultsAreReturned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var result = await _getModulesAction.Execute(baseUrl);

            Assert.NotNull(result);
        }

        public void InitializeFakeObjects()
        {
            _httpClientFactoryStub = new Mock<IHttpClientFactory>();
            _getModulesAction = new GetModulesAction(_httpClientFactoryStub.Object);
            _updateModuleAction = new UpdateUnitsAction(_httpClientFactoryStub.Object);
        }
    }
}