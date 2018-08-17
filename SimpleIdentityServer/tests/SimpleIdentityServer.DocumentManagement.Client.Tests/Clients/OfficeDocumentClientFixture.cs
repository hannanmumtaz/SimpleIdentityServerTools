using Moq;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments;
using SimpleIdentityServer.DocumentManagement.Client.Tests.Middlewares;
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
            _officeDocumentClient = new OfficeDocumentClient(updateOfficeDocumentOperation, getOfficeDocumentOperation,
                addOfficeDocumentOperation, decryptOfficeDocumentOperation, getConfigurationOperation, getPermissionsOperation);
        }
    }
}
