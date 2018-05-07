using Moq;
using SimpleIdentityServer.Core.Repositories;
using SimpleIdentityServer.Eid.OpenId.Core.Login.Actions;
using SimpleIdentityServer.Eid.OpenId.Core.Parameters;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Eid.Tests.Login.Actions
{
    public class LocalLoginActionFixture
    {
        private Mock<IResourceOwnerRepository> _resourceOwnerRepositoryStub;
        private ILocalAuthenticateAction _localAuthenticateAction;

        [Fact]
        public async Task WhenPassingNullParameterThenExceptionsAreThrown()
        {
            InitializeFakeObjects();
            await Assert.ThrowsAsync<ArgumentNullException>(() => _localAuthenticateAction.Execute(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _localAuthenticateAction.Execute(new LocalAuthenticateParameter()));
        }

        [Fact]
        public async Task WhenAuthenticateUserThenResourceOwnerIsReturned()
        {
            // ARRANGE
            InitializeFakeObjects();
            var xml = File.ReadAllText("SamlRequest.XML");

            // ACT
            var resourceOwner = await _localAuthenticateAction.Execute(new LocalAuthenticateParameter
            {
                Xml = xml
            });

            // ASSERT
            Assert.NotNull(resourceOwner);
            Assert.Equal("89100739573", resourceOwner.Id);
        }

        private void InitializeFakeObjects()
        {
            _resourceOwnerRepositoryStub = new Mock<IResourceOwnerRepository>();
            _localAuthenticateAction = new LocalAuthenticateAction(_resourceOwnerRepositoryStub.Object);
        }
    }
}
