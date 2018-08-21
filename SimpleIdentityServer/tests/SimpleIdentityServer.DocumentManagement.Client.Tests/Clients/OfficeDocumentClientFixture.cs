using Moq;
using SimpleIdentityServer.Client.Policy;
using SimpleIdentityServer.Client.ResourceSet;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Core.Common.DTOs.Responses;
using SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments;
using SimpleIdentityServer.DocumentManagement.Client.Tests.Middlewares;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
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
        public async Task When_Add_OfficeDocument_And_Pass_No_DisplayName_Then_Error_Is_Returned()
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
            Assert.Equal("parameter 'display_name' is missing", result.Error.ErrorDescription);
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
                Id = "id",
                DisplayName = "display_name"
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
                Id = "id",
                DisplayName = "display_name"
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
                Id = "newdocumentid",
                DisplayName = "displayname"
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
                Id = "newdocumentid",
                DisplayName = "displayname"
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
                Id = "newdocumentid",
                DisplayName = "displayname"
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

        [Fact]
        public async Task When_Validate_ConfirmationCode_And_No_Subject_Is_Passed_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = null;
            var result = await _officeDocumentClient.ValidateInvitationLinkResolve("confirmationCode", $"{baseUrl}/configuration", "token");

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("invalid_request", result.Error.Error);
            Assert.Equal("the subject is missing", result.Error.ErrorDescription);
        }
        
        [Fact]
        public async Task When_Validate_Confirmation_Code_And_Doesnt_Exist_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = "subject";
            var result = await _officeDocumentClient.ValidateInvitationLinkResolve("confirmationCode", $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("internal_error", result.Error.Error);
            Assert.Equal("the confirmation code is not valid", result.Error.ErrorDescription);
        }

        #endregion

        #region Get all links

        [Fact]
        public async Task When_Get_AllLinks_And_Pass_No_Subject_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = null;
            var result = await _officeDocumentClient.GetAllInvitationLinksResolve("not_valid",  $"{baseUrl}/configuration", "token");

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("invalid_request", result.Error.Error);
            Assert.Equal("the subject is missing", result.Error.ErrorDescription);
        }

        [Fact]
        public async Task When_GetAllLinks_And_Document_Doesnt_Exist_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = "not_valid_sub";
            var result = await _officeDocumentClient.GetAllInvitationLinksResolve("not_valid", $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("invalid_request", result.Error.Error);
            Assert.Equal("the document doesn't exist", result.Error.ErrorDescription);
        }

        [Fact]
        public async Task When_GetAllLink_And_User_Not_Authorized_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = "not_valid_sub";
            var result = await _officeDocumentClient.GetAllInvitationLinksResolve("id", $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal(HttpStatusCode.Unauthorized, result.HttpStatus);
        }

        #endregion

        #region Delete confirmation link

        [Fact]
        public async Task When_Delete_Confirmation_Link_And_Pass_No_Subject_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = null;
            var result = await _officeDocumentClient.DeleteConfirmationLinkResolve("confirmationcode", $"{baseUrl}/configuration", "token");

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal("invalid_request", result.Error.Error);
            Assert.Equal("the subject is missing", result.Error.ErrorDescription);
        }

        [Fact]
        public async Task When_Delete_Confirmation_Link_And_Code_Doesnt_Exist_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = "sub";
            var result = await _officeDocumentClient.DeleteConfirmationLinkResolve("confirmationcode", $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERTS
            Assert.True(result.ContainsError);
            Assert.Equal("invalid_request", result.Error.Error);
            Assert.Equal("the confirmation code doesn't exist", result.Error.ErrorDescription);
        }

        [Fact]
        public async Task When_Delete_Confirmation_Link_And_User_Not_Authorized_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);
            UserStore.Instance().Subject = "subject";
            var result = await _officeDocumentClient.GetInvitationLinkResolve("id", new Common.DTOs.Requests.GenerateConfirmationCodeRequest
            {

            }, $"{baseUrl}/configuration", "token");

            // ACT
            UserStore.Instance().Subject = "invalid_sub";
            var deleteResult = await _officeDocumentClient.DeleteConfirmationLinkResolve(result.Content.ConfirmationCode, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERTS
            Assert.True(deleteResult.ContainsError);
            Assert.Equal(HttpStatusCode.Unauthorized, deleteResult.HttpStatus);
        }

        #endregion

        #region Get invitation link information

        [Fact]
        public async Task When_Get_InvitationLinkInformation_And_Confirmation_Code_Doesnt_Exist_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var result = await _officeDocumentClient.GetInvitationLinkInformationResolve("code", $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.True(result.ContainsError);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        }

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
                Id = "newdocumentid",
                DisplayName = "displayname"
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

        #region Validate link
                
        [Fact]
        public async Task When_Validate_Confirmation_Code_Then_Ok_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);
            UserStore.Instance().Subject = "subject";
            var result = await _officeDocumentClient.GetInvitationLinkResolve("id", new Common.DTOs.Requests.GenerateConfirmationCodeRequest
            {

            }, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = "other_subject";
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
            policyClient.Setup(r => r.GetByResolution(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new GetPolicyResult
            {
                ContainsError = false,
                Content = new PolicyResponse
                {
                    Rules = new List<PolicyRuleResponse>
                    {
                        new PolicyRuleResponse
                        {
                            Claims = new List<PostClaim>
                            {
                                new PostClaim
                                {
                                    Type = "type",
                                    Value = "val"
                                }
                            }
                        }
                    }
                }
            }));
            policyClient.Setup(r => r.UpdateByResolution(It.IsAny<PutPolicy>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new BaseResponse
            {
                ContainsError = false
            }));
            _server.SharedCtx.IdentityServerUmaClientFactory.Setup(a => a.GetResourceSetClient()).Returns(resourceSetClient.Object);
            _server.SharedCtx.IdentityServerUmaClientFactory.Setup(a => a.GetPolicyClient()).Returns(policyClient.Object);

            // ACT
            var validationResult = await _officeDocumentClient.ValidateInvitationLinkResolve(result.Content.ConfirmationCode, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.False(validationResult.ContainsError);
            var confirmationCode = await _server.SharedCtx.OfficeDocumentConfirmationLinkStore.Get(result.Content.ConfirmationCode);
            Assert.Null(confirmationCode);
        }

        [Fact]
        public async Task When_Validate_One_Confirmation_Code_And_There_Are_Three_Then_Ok_Is_Returned()
        {

            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);
            UserStore.Instance().Subject = "subject";
            var result = await _officeDocumentClient.GetInvitationLinkResolve("id", new Common.DTOs.Requests.GenerateConfirmationCodeRequest
            {
                NumberOfConfirmations = 3
            }, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = "other_subject";
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
            policyClient.Setup(r => r.GetByResolution(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new GetPolicyResult
            {
                ContainsError = false,
                Content = new PolicyResponse
                {
                    Rules = new List<PolicyRuleResponse>
                    {
                        new PolicyRuleResponse
                        {
                            Claims = new List<PostClaim>
                            {
                                new PostClaim
                                {
                                    Type = "type",
                                    Value = "val"
                                }
                            }
                        }
                    }
                }
            }));
            policyClient.Setup(r => r.UpdateByResolution(It.IsAny<PutPolicy>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new BaseResponse
            {
                ContainsError = false
            }));
            _server.SharedCtx.IdentityServerUmaClientFactory.Setup(a => a.GetResourceSetClient()).Returns(resourceSetClient.Object);
            _server.SharedCtx.IdentityServerUmaClientFactory.Setup(a => a.GetPolicyClient()).Returns(policyClient.Object);

            // ACT
            var validationResult = await _officeDocumentClient.ValidateInvitationLinkResolve(result.Content.ConfirmationCode, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.False(validationResult.ContainsError);
            var confirmationCode = await _server.SharedCtx.OfficeDocumentConfirmationLinkStore.Get(result.Content.ConfirmationCode);
            Assert.NotNull(confirmationCode);
            Assert.Equal(2, confirmationCode.NumberOfConfirmations.Value);
        }

        #endregion

        #region Get all

        [Fact]
        public async Task When_GetAllConfirmationLinks_Then_Ok_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = "subject";
            var result = await _officeDocumentClient.GetAllInvitationLinksResolve("id", $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.False(result.ContainsError);
        }

        #endregion

        #region Delete confirmation link

        [Fact]
        public async Task When_Delete_Confirmation_Link_Then_Ok_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);
            UserStore.Instance().Subject = "subject";
            var result = await _officeDocumentClient.GetInvitationLinkResolve("id", new Common.DTOs.Requests.GenerateConfirmationCodeRequest
            {

            }, $"{baseUrl}/configuration", "token");

            // ACT
            var deleteResult = await _officeDocumentClient.DeleteConfirmationLinkResolve(result.Content.ConfirmationCode, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERTS
            Assert.False(result.ContainsError);
        }

        #endregion

        #region Get invitation link information

        [Fact]
        public async Task When_Get_InvitationLinkInformation_Then_Ok_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);
            UserStore.Instance().Subject = "subject";
            var result = await _officeDocumentClient.GetInvitationLinkResolve("id", new Common.DTOs.Requests.GenerateConfirmationCodeRequest
            {

            }, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ACT
            var getInvitationLinkInformationResponse = await _officeDocumentClient.GetInvitationLinkInformationResolve(result.Content.ConfirmationCode, $"{baseUrl}/configuration", "token");

            // ASSERT
            Assert.False(getInvitationLinkInformationResponse.ContainsError);
        }

        #endregion

        #region Search documents

        [Fact]
        public async Task When_Search_OfficeDocuments_Then_Ok_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            UserStore.Instance().Subject = "subject";
            var result = await _officeDocumentClient.SearchResolve(new SearchOfficeDocumentRequest
            {
                Count = 100,
                StartIndex = 0
            }, $"{baseUrl}/configuration", "token");
            UserStore.Instance().Subject = null;

            // ASSERT
            Assert.False(result.ContainsError);
        }

        #endregion

        #endregion

        private void InitializeFakeObjects()
        {
            _httpClientFactoryStub = new Mock<IHttpClientFactory>();
            var getOfficeDocumentOperation = new GetOfficeDocumentOperation(_httpClientFactoryStub.Object);
            var addOfficeDocumentOperation = new AddOfficeDocumentOperation(_httpClientFactoryStub.Object);
            var decryptOfficeDocumentOperation = new DecryptOfficeDocumentOperation(_httpClientFactoryStub.Object);
            var getConfigurationOperation = new GetConfigurationOperation(_httpClientFactoryStub.Object);
            var getPermissionsOperation = new GetPermissionsOperation(_httpClientFactoryStub.Object);
            var getInvitationLinkOperation = new GetInvitationLinkOperation(_httpClientFactoryStub.Object);
            var getAllInvitationLinksOperation = new GetAllInvitationLinksOperation(_httpClientFactoryStub.Object);
            var validateConfirmationLinkOperation = new ValidateConfirmationLinkOperation(_httpClientFactoryStub.Object);
            var deleteOfficeDocumentConfirmationCodeOperation = new DeleteOfficeDocumentConfirmationCodeOperation(_httpClientFactoryStub.Object);
            var getInvitationLinkInformationOperation = new GetInvitationLinkInformationOperation(_httpClientFactoryStub.Object);
            var searchOfficeDocumentsOperation = new SearchOfficeDocumentsOperation(_httpClientFactoryStub.Object);
            _officeDocumentClient = new OfficeDocumentClient(getOfficeDocumentOperation,
                addOfficeDocumentOperation, decryptOfficeDocumentOperation, getConfigurationOperation, getPermissionsOperation,
                getInvitationLinkOperation, validateConfirmationLinkOperation, getAllInvitationLinksOperation, deleteOfficeDocumentConfirmationCodeOperation,
                getInvitationLinkInformationOperation, searchOfficeDocumentsOperation);
        }
    }
}