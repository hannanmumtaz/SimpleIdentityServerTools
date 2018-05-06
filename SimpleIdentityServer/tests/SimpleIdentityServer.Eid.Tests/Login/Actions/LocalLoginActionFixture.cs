using SimpleIdentityServer.Eid.OpenId.Core.Login.Actions;
using SimpleIdentityServer.Eid.OpenId.Core.Parameters;
using System;
using System.IO;
using Xunit;

namespace SimpleIdentityServer.Eid.Tests.Login.Actions
{
    public class LocalLoginActionFixture
    {
        private ILocalAuthenticateAction _localAuthenticateAction;

        [Fact]
        public void WhenPassingNullParameterThenExceptionsAreThrown()
        {
            InitializeFakeObjects();
            Assert.Throws<ArgumentNullException>(() => _localAuthenticateAction.Execute(null));
            Assert.Throws<ArgumentNullException>(() => _localAuthenticateAction.Execute(new LocalAuthenticateParameter()));
        }

        [Fact]
        public void WhenAuthenticateUserThenResourceOwnerIsReturned()
        {
            // ARRANGE
            InitializeFakeObjects();
            var xml = File.ReadAllText("SamlRequest.XML");

            // ACT
            var resourceOwner = _localAuthenticateAction.Execute(new LocalAuthenticateParameter
            {
                Xml = xml
            });

            // ASSERT
            Assert.NotNull(resourceOwner);
            Assert.Equal("89100739573", resourceOwner.Id);
        }

        private void InitializeFakeObjects()
        {
            _localAuthenticateAction = new LocalAuthenticateAction();
        }
    }
}
