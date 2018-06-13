using Moq;
using SimpleIdentityServer.Module.Feed.Core.Connectors.Actions;
using SimpleIdentityServer.Module.Feed.Core.Exceptions;
using SimpleIdentityServer.Module.Feed.Core.Models;
using SimpleIdentityServer.Module.Feed.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Module.Feed.Core.Tests.Connectors
{
    public class AddConnectorActionFixture
    {
        private Mock<IConnectorRepository> _connectorRepositoryStub;
        private IAddConnectorAction _addConnectorAction;

        [Fact]
        public async Task WhenNullParametersArePassedThenExceptionsAreThrown()
        {
            // ARRANGE
            InitializeFakeObjects();

            // ACTS & ASSERTS
            await Assert.ThrowsAsync<ArgumentNullException>(() => _addConnectorAction.Execute(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _addConnectorAction.Execute(new ConnectorAggregate()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _addConnectorAction.Execute(new ConnectorAggregate { Name = "name" }));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _addConnectorAction.Execute(new ConnectorAggregate { Name = "name", Library = "lib" }));
        }

        [Fact]
        public async Task WhenConnectorAlreadyExistsThenExceptionIsThrown()
        {
            // ARRANGE
            InitializeFakeObjects();
            _connectorRepositoryStub.Setup(c => c.Get(It.IsAny<string>())).Returns(Task.FromResult(new ConnectorAggregate()));

            // ACT
            var exception = await Assert.ThrowsAsync<ModuleFeedInternalException>(() => _addConnectorAction.Execute(new ConnectorAggregate { Name = "name", Library = "lib", Version = "version" }));

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal("internal", exception.Code);
            Assert.Equal("connector_already_exists", exception.Message);
        }

        [Fact]
        public async Task WhenConnectorIsInsertedThenAddIsCalled()
        {
            // ARRANGE
            var connectorAggregate = new ConnectorAggregate { Name = "name", Library = "lib", Version = "version" };
            InitializeFakeObjects();
            _connectorRepositoryStub.Setup(c => c.Get(It.IsAny<string>())).Returns(Task.FromResult((ConnectorAggregate)null));

            // ACT
            await _addConnectorAction.Execute(connectorAggregate);

            // ASSERTS
            _connectorRepositoryStub.Verify(c => c.Add(It.Is<IEnumerable<ConnectorAggregate>>(p => p.First() == connectorAggregate)), Times.Once);
        }

        private void InitializeFakeObjects()
        {
            _connectorRepositoryStub = new Mock<IConnectorRepository>();
            _addConnectorAction = new AddConnectorAction(_connectorRepositoryStub.Object);
        }
    }
}
