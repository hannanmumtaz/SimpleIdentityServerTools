using Moq;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Manager.Client.Configuration;
using SimpleIdentityServer.Manager.Client.ResourceOwners;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Manager.Host.Tests
{
    public class ResourceOwnerFixture : IClassFixture<TestManagerServerFixture>
    {
        private TestManagerServerFixture _server;
        private Mock<IHttpClientFactory> _httpClientFactoryStub;
        private IResourceOwnerClient _resourceOwnerClient;

        public ResourceOwnerFixture(TestManagerServerFixture server)
        {
            _server = server;
        }

        #region Errors

        [Fact]
        public async Task When_Update_Claims_And_No_Login_Is_Passwed_Then_Error_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var result = await _resourceOwnerClient.ResolveUpdateClaims(new Uri("http://localhost:5000/.well-known/openidmanager-configuration"), new Common.Requests.UpdateResourceOwnerClaimsRequest(), null);

            // ASSERT
            Assert.NotNull(result);
            Assert.True(result.ContainsError);
            Assert.Equal("invalid_request", result.Error.Error);
            Assert.Equal("the parameter login is missing", result.Error.ErrorDescription);
        }

        [Fact]
        public async Task When_Update_Password_And_No_Login_Is_Passwed_Then_Error_Is_Returned()
        {

        }

        [Fact]
        public async Task When_Update_Claims_And_No_Password_Is_Passwed_Then_Error_Is_Returned()
        {

        }

        [Fact]
        public async Task When_Update_Claims_And_Resource_Owner_Doesnt_Exist_Then_Error_Is_Returned()
        {

        }
        [Fact]
        public async Task When_Update_Password_And_Resource_Owner_Doesnt_Exist_Then_Error_Is_Returned()
        {

        }

        #endregion

        #region Happy paths

        [Fact]
        public async Task When_Search_Resource_Owners_Then_One_Resource_Owner_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var result = await _resourceOwnerClient.ResolveSearch(new Uri("http://localhost:5000/.well-known/openidmanager-configuration"), new Common.Requests.SearchResourceOwnersRequest
            {
                StartIndex = 0,
                NbResults = 1
            }, null);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(1, result.Content.Content.Count());
            Assert.False(result.ContainsError);
        }

        #endregion

        private void InitializeFakeObjects()
        {
            _httpClientFactoryStub = new Mock<IHttpClientFactory>();
            var addResourceOwnerOperation = new AddResourceOwnerOperation(_httpClientFactoryStub.Object);
            var deleteResourceOwnerOperation = new DeleteResourceOwnerOperation(_httpClientFactoryStub.Object);
            var getAllResourceOwnersOperation = new GetAllResourceOwnersOperation(_httpClientFactoryStub.Object);
            var getResourceOwnerOperation = new GetResourceOwnerOperation(_httpClientFactoryStub.Object);
            var updateResourceOwnerClaimsOperation = new UpdateResourceOwnerClaimsOperation(_httpClientFactoryStub.Object);
            var updateResourceOwnerPasswordOperation = new UpdateResourceOwnerPasswordOperation(_httpClientFactoryStub.Object);
            var getConfigurationOperation = new GetConfigurationOperation(_httpClientFactoryStub.Object);
            var configurationClient = new ConfigurationClient(getConfigurationOperation);
            var searchResourceOwnersOperation = new SearchResourceOwnersOperation(_httpClientFactoryStub.Object);
            _resourceOwnerClient = new ResourceOwnerClient(addResourceOwnerOperation, deleteResourceOwnerOperation,
                getAllResourceOwnersOperation, getResourceOwnerOperation, updateResourceOwnerClaimsOperation, updateResourceOwnerPasswordOperation, configurationClient, searchResourceOwnersOperation);
        }
    }
}
