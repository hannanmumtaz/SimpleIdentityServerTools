using Moq;
using SimpleIdentityServer.AccessToken.Store;
using SimpleIdentityServer.Core.Common.DTOs.Responses;
using SimpleIdentityServer.Core.Common.Models;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.DocumentManagement.Core.Tests.OfficeDocuments.Actions
{
    public class AddDocumentActionFixture
    {
        [Fact]
        public async Task WhenPassingNullParametersThenExceptionsAreThrown()
        {
            // ARRANGE
            var officeDocumentAction = new AddOfficeDocumentAction(null, null, null);

            // ACTS & ASSERTS
            await Assert.ThrowsAsync<ArgumentNullException>(() => officeDocumentAction.Execute(null, null, null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => officeDocumentAction.Execute("openid", null, null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => officeDocumentAction.Execute("openid", new OfficeDocumentAggregate(), null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => officeDocumentAction.Execute("openid", new OfficeDocumentAggregate(), new AuthenticateParameter()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => officeDocumentAction.Execute("openid", new OfficeDocumentAggregate("id", null), new AuthenticateParameter()));
        }

        [Fact]
        public async Task WhenOfficeDocumentAlreadyExistsThenExceptionIsThrown()
        {
            // ARRANGE
            var officeDocumentRepositoryStub = new Mock<IOfficeDocumentRepository>();
            officeDocumentRepositoryStub.Setup(r => r.Get(It.IsAny<string>())).Returns(Task.FromResult(new OfficeDocumentAggregate()));
            var officeDocumentAction = new AddOfficeDocumentAction(officeDocumentRepositoryStub.Object, null, null);

            // ACT
            var exception = await Assert.ThrowsAsync<InternalDocumentException>(() => officeDocumentAction.Execute("openid", new OfficeDocumentAggregate("id", "sub"), new AuthenticateParameter()));

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal("internal", exception.Code);
            Assert.Equal("document_exists", exception.Message);
        }

        [Fact]
        public async Task WhenAccessTokenCannotBeRetrievedThenExceptionIsThrown()
        {
            // ARRANGE
            var officeDocumentRepositoryStub = new Mock<IOfficeDocumentRepository>();
            var tokenStoreStub = new Mock<IAccessTokenStore>();
            officeDocumentRepositoryStub.Setup(r => r.Get(It.IsAny<string>())).Returns(Task.FromResult((OfficeDocumentAggregate)null));
            tokenStoreStub.Setup(r => r.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).Returns(Task.FromResult(new GrantedTokenResponse()));
            var officeDocumentAction = new AddOfficeDocumentAction(officeDocumentRepositoryStub.Object, null, tokenStoreStub.Object);

            // ACT
            var exception = await Assert.ThrowsAsync<InvalidConfigurationException>(() => officeDocumentAction.Execute("openid", new OfficeDocumentAggregate("id", "sub"), new AuthenticateParameter()));

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal("invalid_client_configuration", exception.Message);
        }
    }
}
