using Moq;
using SimpleIdentityServer.Module.Feed.Core.Connectors.Actions;
using SimpleIdentityServer.Module.Feed.Core.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Module.Feed.Core.Tests.Connectors
{
    public class GetAllConnectorsActionFixture
    {
        private Mock<IConnectorRepository> _connectorRepositoryStub;
        private IGetAllConnectorsAction _getAllConnectorsAction;

        [Fact]
        public async Task WhenGetAllConnectorsThenOperationIsCalled()
        {
            // ARRANGE
            InitializeFakeObjects();

            // ACT
            await _getAllConnectorsAction.Execute();

            // ASSERT
            _connectorRepositoryStub.Verify(c => c.GetAll());
        }

        private void InitializeFakeObjects()
        {
            _connectorRepositoryStub = new Mock<IConnectorRepository>();
            _getAllConnectorsAction = new GetAllConnectorsAction(_connectorRepositoryStub.Object);
        }
    }
}
