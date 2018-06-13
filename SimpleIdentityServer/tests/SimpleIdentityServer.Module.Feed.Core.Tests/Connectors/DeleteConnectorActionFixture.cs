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
    public class DeleteConnectorActionFixture
    {
        private Mock<IConnectorRepository> _connectorRepositoryStub;
        private IDeleteConnectorAction _deleteConnectorAction;

        [Fact]
        public async Task WhenNullParametersArePassedThenExceptionsAreThrown()
        {
            // ARRANGE
            InitializeFakeObjects();

            // ACTS & ASSERTS
            await Assert.ThrowsAsync<ArgumentNullException>(() => _deleteConnectorAction.Execute(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _deleteConnectorAction.Execute(string.Empty));
        }

        [Fact]
        public async Task WhenConnectorDoesntExistThenExceptionIsThrown()
        {
            // ARRANGE
            InitializeFakeObjects();
            _connectorRepositoryStub.Setup(c => c.Get(It.IsAny<string>())).Returns(Task.FromResult((ConnectorAggregate)null));

            // ACT
            var exception = await Assert.ThrowsAsync<NoConnectorException>(() => _deleteConnectorAction.Execute("name"));

            // ASSERTS
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task WhenConnectorIsDeleteThenDeleteIsCalled()
        {
            // ARRANGE
            const string name = "name";
            InitializeFakeObjects();
            _connectorRepositoryStub.Setup(c => c.Get(It.IsAny<string>())).Returns(Task.FromResult(new ConnectorAggregate { Name = name }));

            // ACT
            await _deleteConnectorAction.Execute(name);

            // ASSERTS
            _connectorRepositoryStub.Verify(c => c.Delete(It.Is<IEnumerable<string>>(p => p.First() == name)), Times.Once);
        }

        private void InitializeFakeObjects()
        {
            _connectorRepositoryStub = new Mock<IConnectorRepository>();
            _deleteConnectorAction = new DeleteConnectorAction(_connectorRepositoryStub.Object);
        }
    }
}
