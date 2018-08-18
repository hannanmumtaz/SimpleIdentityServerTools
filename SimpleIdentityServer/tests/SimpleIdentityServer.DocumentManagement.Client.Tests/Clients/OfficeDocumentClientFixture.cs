using Moq;
using SimpleIdentityServer.Client.Policy;
using SimpleIdentityServer.Client.ResourceSet;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Core.Common.DTOs.Responses;
using SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments;
using SimpleIdentityServer.DocumentManagement.Client.Tests.Middlewares;
using SimpleIdentityServer.Uma.Client.Results;
using SimpleIdentityServer.Uma.Common.DTOs;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.DocumentManagement.Client.Tests.Clients
{
    public class OfficeDocumentClientFixture : IClassFixture<TestDocumentManagementServerFixture>
    {
        private const string baseUrl = "http://localhost:5000";
        private readonly TestDocumentManagementServerFixture _server;
        private Mock<IHttpClientFactory> _httpClientFactoryStub;
        private IOfficeDocumentClient _officeDocumentClient;

        public OfficeDocumentClientFixture(TestDocumentManagementServerFixture server)
        {
            _server = server;
        }

        #region Errors

        #region Add

        [Fact]
        public async Task When_Add_OfficeDocument_And_Pass_No_Parameter_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var result = await _officeDocumentClient.AddResolve(new Common.DTOs.Requests.AddOfficeDocumentRequest
            {

            }, $"{baseUrl}/configuration", "token");

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("invalid_request", result.Error.Error);
            Assert.Equal("parameter 'id' is missing", result.Error.ErrorDescription);
        }

        [Fact]
        public async Task When_Add_OfficeDocument_And_Pass_No_Subject_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var result = await _officeDocumentClient.AddResolve(new Common.DTOs.Requests.AddOfficeDocumentRequest
            {
                Id = "id"
            }, $"{baseUrl}/configuration", "token");

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("invalid_request", result.Error.Error);
            Assert.Equal("the subject is missing", result.Error.ErrorDescription);
        }

        [Fact]
        public async Task When_Add_OfficeDocument_And_Pass_Invalid_DocumentId_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = "sub";
            var result = await _officeDocumentClient.AddResolve(new Common.DTOs.Requests.AddOfficeDocumentRequest
            {
                Id = "id"
            }, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("internal_error", result.Error.Error);
            Assert.Equal("office document already exists", result.Error.ErrorDescription);
        }

        [Fact]
        public async Task When_Add_OfficeDocument_And_AccessToken_Cannot_Be_Retrieved_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);
            _server.SharedCtx.AccessTokenStore.Setup(a => a.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult((GrantedTokenResponse)null));

            // ACT
            UserStore.Instance().Subject = "sub";
            var result = await _officeDocumentClient.AddResolve(new Common.DTOs.Requests.AddOfficeDocumentRequest
            {
                Id = "newdocumentid"
            }, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("internal_error", result.Error.Error);
            Assert.Equal("an error occured while trying to get an access token", result.Error.ErrorDescription);
        }

        [Fact]
        public async Task When_Add_OfficeDocument_And_UmaResource_Cannot_Be_Added_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);
            _server.SharedCtx.AccessTokenStore.Setup(a => a.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(new GrantedTokenResponse
                {
                    AccessToken = "access_token"
                }));
            var resourceSetClient = new Mock<IResourceSetClient>();
            resourceSetClient.Setup(r => r.AddByResolution(It.IsAny<PostResourceSet>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new AddResourceSetResult
            {
                ContainsError = true
            }));
            _server.SharedCtx.IdentityServerUmaClientFactory.Setup(a => a.GetResourceSetClient()).Returns(resourceSetClient.Object);
            // ACT
            UserStore.Instance().Subject = "sub";
            var result = await _officeDocumentClient.AddResolve(new Common.DTOs.Requests.AddOfficeDocumentRequest
            {
                Id = "newdocumentid"
            }, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("internal_error", result.Error.Error);
            Assert.Equal("an error occured while trying to add the UMA resource", result.Error.ErrorDescription);
        }

        [Fact]
        public async Task When_Add_Office_Document_And_UmaPolicy_Cannot_Be_Added_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);
            _server.SharedCtx.AccessTokenStore.Setup(a => a.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(new GrantedTokenResponse
                {
                    AccessToken = "access_token"
                }));
            var resourceSetClient = new Mock<IResourceSetClient>();
            var policyClient = new Mock<IPolicyClient>();
            resourceSetClient.Setup(r => r.AddByResolution(It.IsAny<PostResourceSet>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new AddResourceSetResult
            {
                ContainsError = false,
                Content = new AddResourceSetResponse
                {
                    Id = "id"
                }
            }));
            policyClient.Setup(r => r.AddByResolution(It.IsAny<PostPolicy>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new AddPolicyResult
            {
                ContainsError = true
            }));
            _server.SharedCtx.IdentityServerUmaClientFactory.Setup(a => a.GetResourceSetClient()).Returns(resourceSetClient.Object);
            _server.SharedCtx.IdentityServerUmaClientFactory.Setup(a => a.GetPolicyClient()).Returns(policyClient.Object);
            // ACT
            UserStore.Instance().Subject = "sub";
            var result = await _officeDocumentClient.AddResolve(new Common.DTOs.Requests.AddOfficeDocumentRequest
            {
                Id = "newdocumentid"
            }, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("internal_error", result.Error.Error);
            Assert.Equal("an error occured while trying to add the UMA policy", result.Error.ErrorDescription);
        }

        #endregion

        #region Get link

        [Fact]
        public async Task When_Get_Confirmation_Link_And_Subject_Doesnt_Exist_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = null;
            var result = await _officeDocumentClient.GetInvitationLinkResolve("invalid_document_id", new Common.DTOs.Requests.GenerateConfirmationCodeRequest
            {
                
            }, $"{baseUrl}/configuration", "token");

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("invalid_request", result.Error.Error);
            Assert.Equal("the subject is missing", result.Error.ErrorDescription);
        }

        [Fact]
        public async Task When_Get_Confirmation_Link_And_Document_Doesnt_Exist_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = "subject";
            var result = await _officeDocumentClient.GetInvitationLinkResolve("invalid_document_id", new Common.DTOs.Requests.GenerateConfirmationCodeRequest
            {

            }, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("invalid_request", result.Error.Error);
            Assert.Equal("the document doesn't exist", result.Error.ErrorDescription);
        }

        [Fact]
        public async Task When_Get_Confirmation_Link_And_Subject_Is_Different_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = "invalid_subject";
            var result = await _officeDocumentClient.GetInvitationLinkResolve("id", new Common.DTOs.Requests.GenerateConfirmationCodeRequest
            {

            }, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal(HttpStatusCode.Unauthorized, result.HttpStatus);
        }

        #endregion

        #region Validate link



        #endregion

        #endregion

        #region Happy path

        #region Add

        [Fact]
        public async Task When_Add_Office_Document_Then_Ok_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);
            _server.SharedCtx.AccessTokenStore.Setup(a => a.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(new GrantedTokenResponse
                {
                    AccessToken = "access_token"
                }));
            var resourceSetClient = new Mock<IResourceSetClient>();
            var policyClient = new Mock<IPolicyClient>();
            resourceSetClient.Setup(r => r.AddByResolution(It.IsAny<PostResourceSet>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new AddResourceSetResult
            {
                ContainsError = false,
                Content = new AddResourceSetResponse
                {
                    Id = "id"
                }
            }));
            policyClient.Setup(r => r.AddByResolution(It.IsAny<PostPolicy>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new AddPolicyResult
            {
                ContainsError = false,
                Content = new AddPolicyResponse
                {
                    PolicyId = "policyid"
                }
            }));
            _server.SharedCtx.IdentityServerUmaClientFactory.Setup(a => a.GetResourceSetClient()).Returns(resourceSetClient.Object);
            _server.SharedCtx.IdentityServerUmaClientFactory.Setup(a => a.GetPolicyClient()).Returns(policyClient.Object);
            // ACT
            UserStore.Instance().Subject = "sub";
            var result = await _officeDocumentClient.AddResolve(new Common.DTOs.Requests.AddOfficeDocumentRequest
            {
                Id = "newdocumentid"
            }, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.False(result.ContainsError);
        }

        #endregion

        #region Get link

        [Fact]
        public async Task When_Get_Confirmation_Link_Then_Ok_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = "subject";
            var result = await _officeDocumentClient.GetInvitationLinkResolve("id", new Common.DTOs.Requests.GenerateConfirmationCodeRequest
            {

            }, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.False(result.ContainsError);
            Assert.NotNull(result.Content.ConfirmationCode);
        }

        #endregion

        #endregion

        private void InitializeFakeObjects()
        {
            _httpClientFactoryStub = new Mock<IHttpClientFactory>();
            var updateOfficeDocumentOperation = new UpdateOfficeDocumentOperation(_httpClientFactoryStub.Object);
            var getOfficeDocumentOperation = new GetOfficeDocumentOperation(_httpClientFactoryStub.Object);
            var addOfficeDocumentOperation = new AddOfficeDocumentOperation(_httpClientFactoryStub.Object);
            var decryptOfficeDocumentOperation = new DecryptOfficeDocumentOperation(_httpClientFactoryStub.Object);
            var getConfigurationOperation = new GetConfigurationOperation(_httpClientFactoryStub.Object);
            var getPermissionsOperation = new GetPermissionsOperation(_httpClientFactoryStub.Object);
            var getInvitationLinkOperation = new GetInvitationLinkOperation(_httpClientFactoryStub.Object);
            var validateConfirmationLinkOperation = new ValidateConfirmationLinkOperation(_httpClientFactoryStub.Object);
            _officeDocumentClient = new OfficeDocumentClient(updateOfficeDocumentOperation, getOfficeDocumentOperation,
                addOfficeDocumentOperation, decryptOfficeDocumentOperation, getConfigurationOperation, getPermissionsOperation,
                getInvitationLinkOperation, validateConfirmationLinkOperation);
        }
    }
}
