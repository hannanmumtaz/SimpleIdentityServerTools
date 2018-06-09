using Moq;
using SimpleIdentityServer.Client.DTOs.Response;
using SimpleIdentityServer.Common.TokenStore;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Extensions;
using SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
            await Assert.ThrowsAsync<ArgumentNullException>(() => officeDocumentAction.Execute("openid", new OfficeDocumentAggregate("id", null, null, null, null), new AuthenticateParameter()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => officeDocumentAction.Execute("openid", new OfficeDocumentAggregate("id", "sub", null, null, null), new AuthenticateParameter()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => officeDocumentAction.Execute("openid", new OfficeDocumentAggregate("id", "sub", EncAlgorithms.AES, null, null), new AuthenticateParameter()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => officeDocumentAction.Execute("openid", new OfficeDocumentAggregate("id", "sub", EncAlgorithms.AES, "pass", null), new AuthenticateParameter()));
        }

        [Fact]
        public async Task WhenOfficeDocumentAlreadyExistsThenExceptionIsThrown()
        {
            // ARRANGE
            var officeDocumentRepositoryStub = new Mock<IOfficeDocumentRepository>();
            officeDocumentRepositoryStub.Setup(r => r.Get(It.IsAny<string>())).Returns(Task.FromResult(new OfficeDocumentAggregate()));
            var officeDocumentAction = new AddOfficeDocumentAction(officeDocumentRepositoryStub.Object, null, null);

            // ACT
            var exception = await Assert.ThrowsAsync<InternalDocumentException>(() => officeDocumentAction.Execute("openid", new OfficeDocumentAggregate("id", "sub", EncAlgorithms.AES, "pass", "salt"), new AuthenticateParameter()));

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
            var tokenStoreStub = new Mock<ITokenStore>();
            officeDocumentRepositoryStub.Setup(r => r.Get(It.IsAny<string>())).Returns(Task.FromResult((OfficeDocumentAggregate)null));
            tokenStoreStub.Setup(r => r.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).Returns(Task.FromResult(new GrantedToken()));
            var officeDocumentAction = new AddOfficeDocumentAction(officeDocumentRepositoryStub.Object, null, tokenStoreStub.Object);

            // ACT
            var exception = await Assert.ThrowsAsync<InvalidConfigurationException>(() => officeDocumentAction.Execute("openid", new OfficeDocumentAggregate("id", "sub", EncAlgorithms.AES, "pass", "salt"), new AuthenticateParameter()));

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal("invalid_client_configuration", exception.Message);
        }
    }
}
