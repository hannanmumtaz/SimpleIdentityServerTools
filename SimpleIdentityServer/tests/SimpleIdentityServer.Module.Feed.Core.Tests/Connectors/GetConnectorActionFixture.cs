using Moq;
using SimpleIdentityServer.Module.Feed.Core.Connectors.Actions;
using SimpleIdentityServer.Module.Feed.Core.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Module.Feed.Core.Tests.Connectors
{
    public class GetConnectorActionFixture
    {
        private Mock<IConnectorRepository> _connectorRepositoryStub;
        private IGetConnectorAction _getConnectorAction;

        [Fact]
        public async Task WhenNullParameterIsPassedThenExceptionIsThrown()
        {
            // ARRANGE
            InitializeFakeObjects();

            // ACTS & ASSERTS
            await Assert.ThrowsAsync<ArgumentNullException>(() => _getConnectorAction.Execute(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _getConnectorAction.Execute(string.Empty));
        }

        [Fact]
        public async Task WhenGetConnectorThenOperationIsCalled()
        {
            const string name = "name";
            // ARRANGE
            InitializeFakeObjects();

            // ACT
            await _getConnectorAction.Execute(name);

            // ASSERT
            _connectorRepositoryStub.Verify(c => c.Get(name));
        }

        private void InitializeFakeObjects()
        {
            _connectorRepositoryStub = new Mock<IConnectorRepository>();
            _getConnectorAction = new GetConnectorAction(_connectorRepositoryStub.Object);
        }
    }
}
