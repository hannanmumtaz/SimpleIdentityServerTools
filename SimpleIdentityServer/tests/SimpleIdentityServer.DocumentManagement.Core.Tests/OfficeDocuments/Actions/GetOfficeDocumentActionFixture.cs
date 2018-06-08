using Moq;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.DocumentManagement.Core.Tests.OfficeDocuments.Actions
{
    public class GetOfficeDocumentActionFixture
    {
        [Fact]
        public async Task WhenPassingNullParametersThenExceptionsAreThrown()
        {
            // ARRANGE
            var getOfficeDocumentAction = new GetOfficeDocumentAction(null, null, null);

            // ACTS & ASSERTS
            await Assert.ThrowsAsync<ArgumentNullException>(() => getOfficeDocumentAction.Execute(null, null, null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => getOfficeDocumentAction.Execute("documentId", null, null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => getOfficeDocumentAction.Execute("documentId", "accessToken", null));
        }

        [Fact]
        public async Task WhenDocumentDoesntExistThenExceptionIsThrown()
        {
            // ARRANGE
            var officeDocumentRepositoryStub = new Mock<IOfficeDocumentRepository>();
            officeDocumentRepositoryStub.Setup(o => o.Get(It.IsAny<string>())).Returns(Task.FromResult((OfficeDocumentAggregate)null));
            var getOfficeDocumentAction = new GetOfficeDocumentAction(officeDocumentRepositoryStub.Object, null, null);

            // ACT
            var exception = await Assert.ThrowsAsync<DocumentNotFoundException>(() => getOfficeDocumentAction.Execute("documentId", "accessToken", new AuthenticateParameter()));

            // ASSERTS
            Assert.NotNull(exception);
        }
    }
}
