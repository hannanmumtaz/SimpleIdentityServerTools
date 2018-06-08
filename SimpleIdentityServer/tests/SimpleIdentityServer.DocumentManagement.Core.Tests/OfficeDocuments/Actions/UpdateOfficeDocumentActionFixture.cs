using Moq;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Client.DTOs.Response;
using SimpleIdentityServer.Common.TokenStore;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.DocumentManagement.Core.Tests.OfficeDocuments.Actions
{
    public class UpdateOfficeDocumentActionFixture
    {
        [Fact]
        public async Task WhenPassingNullParametersThenExceptionsAreThrown()
        {
            // ARRANGE
            var updateOfficeDocumentAction = new UpdateOfficeDocumentAction(null, null, null);

            // ACTS & ASSERTS
            await Assert.ThrowsAsync<ArgumentNullException>(() => updateOfficeDocumentAction.Execute(null, null, null, null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => updateOfficeDocumentAction.Execute("subject", null, null, null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => updateOfficeDocumentAction.Execute("subject", "documentId", null, null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => updateOfficeDocumentAction.Execute("subject", "documentId", new UpdateOfficeDocumentParameter(), null));
        }

        [Fact]
        public async Task WhenPassingInvalidScopesThenExceptionIsThrown()
        {
            // ARRANGE
            var updateOfficeDocumentAction = new UpdateOfficeDocumentAction(null, null, null);

            // ACT
            var exception = await Assert.ThrowsAsync<InternalDocumentException>(() => updateOfficeDocumentAction.Execute("subject", "documentId", new UpdateOfficeDocumentParameter
            {
                Permissions = new List<OfficeDocumentPermission>
                {
                    new OfficeDocumentPermission
                    {
                        Scopes = new [] { "invalid" }
                    }
                }
            }, new AuthenticateParameter()));

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal("internal", exception.Code);
            Assert.Equal("invalid_scopes", exception.Message);
        }

        [Fact]
        public async Task WhenPassingNoPermissionSubjectThenExceptionIsThrown()
        {
            // ARRANGE
            var updateOfficeDocumentAction = new UpdateOfficeDocumentAction(null, null, null);

            // ACT
            var exception = await Assert.ThrowsAsync<InternalDocumentException>(() => updateOfficeDocumentAction.Execute("subject", "documentId", new UpdateOfficeDocumentParameter
            {
                Permissions = new List<OfficeDocumentPermission>
                {
                    new OfficeDocumentPermission
                    {
                        Scopes = new [] { "read" }
                    }
                }
            }, new AuthenticateParameter()));

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal("internal", exception.Code);
            Assert.Equal("permission_subject_must_be_specified", exception.Message);
        }

        [Fact]
        public async Task WhenOfficeDocumentDoesntExistThenExceptionIsThrown()
        {
            // ARRANGE
            var officeDocumentRepositoryStub = new Mock<IOfficeDocumentRepository>();
            officeDocumentRepositoryStub.Setup(o => o.Get(It.IsAny<string>())).Returns(Task.FromResult((OfficeDocumentAggregate)null));
            var updateOfficeDocumentAction = new UpdateOfficeDocumentAction(officeDocumentRepositoryStub.Object, null, null);

            // ACT
            var exception = await Assert.ThrowsAsync<InternalDocumentException>(() => updateOfficeDocumentAction.Execute("subject", "documentId", new UpdateOfficeDocumentParameter
            {
                Permissions = new List<OfficeDocumentPermission>
                {
                    new OfficeDocumentPermission
                    {
                        Scopes = new [] { "read" },
                        Subject = "sub"
                    }
                }
            }, new AuthenticateParameter()));

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal("internal", exception.Code);
            Assert.Equal("document_doesnt_exist", exception.Message);
        }

        [Fact]
        public async Task WhenOfficeDocumentDoesntContainUmaResourceIdThenExceptionIsThrown()
        {
            // ARRANGE
            var officeDocumentRepositoryStub = new Mock<IOfficeDocumentRepository>();
            officeDocumentRepositoryStub.Setup(o => o.Get(It.IsAny<string>())).Returns(Task.FromResult(new OfficeDocumentAggregate()));
            var updateOfficeDocumentAction = new UpdateOfficeDocumentAction(officeDocumentRepositoryStub.Object, null, null);

            // ACT
            var exception = await Assert.ThrowsAsync<InternalDocumentException>(() => updateOfficeDocumentAction.Execute("subject", "documentId", new UpdateOfficeDocumentParameter
            {
                Permissions = new List<OfficeDocumentPermission>
                {
                    new OfficeDocumentPermission
                    {
                        Scopes = new [] { "read" },
                        Subject = "sub"
                    }
                }
            }, new AuthenticateParameter()));

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal("internal", exception.Code);
            Assert.Equal("no_uma_resource", exception.Message);
        }

        [Fact]
        public async Task WhenOfficeDocumentDoesntContainUmaPolicyIdThenExceptionIsThrown()
        {
            // ARRANGE
            var officeDocumentRepositoryStub = new Mock<IOfficeDocumentRepository>();
            officeDocumentRepositoryStub.Setup(o => o.Get(It.IsAny<string>())).Returns(Task.FromResult(new OfficeDocumentAggregate { UmaResourceId = "rid" }));
            var updateOfficeDocumentAction = new UpdateOfficeDocumentAction(officeDocumentRepositoryStub.Object, null, null);

            // ACT
            var exception = await Assert.ThrowsAsync<InternalDocumentException>(() => updateOfficeDocumentAction.Execute("subject", "documentId", new UpdateOfficeDocumentParameter
            {
                Permissions = new List<OfficeDocumentPermission>
                {
                    new OfficeDocumentPermission
                    {
                        Scopes = new [] { "read" },
                        Subject = "sub"
                    }
                }
            }, new AuthenticateParameter()));

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal("internal", exception.Code);
            Assert.Equal("no_uma_policy", exception.Message);
        }

        [Fact]
        public async Task WhenUserIsNotAuthorizedToUpdatePermissionsThenExceptionIsThrown()
        {
            // ARRANGE
            var officeDocumentRepositoryStub = new Mock<IOfficeDocumentRepository>();
            officeDocumentRepositoryStub.Setup(o => o.Get(It.IsAny<string>())).Returns(Task.FromResult(new OfficeDocumentAggregate { UmaResourceId = "rid", UmaPolicyId = "policy", Subject = "subTst" }));
            var updateOfficeDocumentAction = new UpdateOfficeDocumentAction(officeDocumentRepositoryStub.Object, null, null);

            // ACT
            var exception = await Assert.ThrowsAsync<NotAuthorizedException>(() => updateOfficeDocumentAction.Execute("subject", "documentId", new UpdateOfficeDocumentParameter
            {
                Permissions = new List<OfficeDocumentPermission>
                {
                    new OfficeDocumentPermission
                    {
                        Scopes = new [] { "read" },
                        Subject = "sub"
                    }
                }
            }, new AuthenticateParameter()));

            // ASSERTS
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task WhenAccessTokenCannotBeRetrievedThenExceptionIsThrown()
        {
            // ARRANGE
            var tokenStoreStub = new Mock<ITokenStore>();
            tokenStoreStub.Setup(r => r.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).Returns(Task.FromResult(new GrantedToken()));
            var officeDocumentRepositoryStub = new Mock<IOfficeDocumentRepository>();
            officeDocumentRepositoryStub.Setup(o => o.Get(It.IsAny<string>())).Returns(Task.FromResult(new OfficeDocumentAggregate { UmaResourceId = "rid", UmaPolicyId = "policy", Subject = "subTst" }));
            var updateOfficeDocumentAction = new UpdateOfficeDocumentAction(officeDocumentRepositoryStub.Object, null, tokenStoreStub.Object);

            // ACT
            var exception = await Assert.ThrowsAsync<InvalidConfigurationException>(() => updateOfficeDocumentAction.Execute("subTst", "documentId", new UpdateOfficeDocumentParameter
            {
                Permissions = new List<OfficeDocumentPermission>
                {
                    new OfficeDocumentPermission
                    {
                        Scopes = new [] { "read" },
                        Subject = "sub"
                    }
                }
            }, new AuthenticateParameter()));

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal("invalid_client_configuration", exception.Message);
        }

        [Fact]
        public async Task WhenUmaPolicyDoesntExistThenExceptionIsThrown()
        {
            // ARRANGE
            var tokenStoreStub = new Mock<ITokenStore>();
            tokenStoreStub.Setup(r => r.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).Returns(Task.FromResult(new GrantedToken() { AccessToken = "access_token" }));
            var officeDocumentRepositoryStub = new Mock<IOfficeDocumentRepository>();
            officeDocumentRepositoryStub.Setup(o => o.Get(It.IsAny<string>())).Returns(Task.FromResult(new OfficeDocumentAggregate { UmaResourceId = "rid", UmaPolicyId = "policy", Subject = "subTst" }));
            var identityServerUmaClientFactoryStub = new Mock<IIdentityServerUmaClientFactory>();
            identityServerUmaClientFactoryStub.Setup(i => i.GetPolicyClient().Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult((PolicyResponse)null));
            var updateOfficeDocumentAction = new UpdateOfficeDocumentAction(officeDocumentRepositoryStub.Object, identityServerUmaClientFactoryStub.Object, tokenStoreStub.Object);

            // ACT
            var exception = await Assert.ThrowsAsync<InternalDocumentException>(() => updateOfficeDocumentAction.Execute("subTst", "documentId", new UpdateOfficeDocumentParameter
            {
                Permissions = new List<OfficeDocumentPermission>
                {
                    new OfficeDocumentPermission
                    {
                        Scopes = new [] { "read" },
                        Subject = "sub"
                    }
                }
            }, new AuthenticateParameter()));

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal("internal", exception.Code);
            Assert.Equal("uma_policy_doesnt_exist", exception.Message);
        }
    }
}
