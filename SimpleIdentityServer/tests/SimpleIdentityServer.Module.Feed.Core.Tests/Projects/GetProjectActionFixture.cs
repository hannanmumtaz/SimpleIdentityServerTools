using Moq;
using SimpleIdentityServer.Module.Feed.Core.Projects.Actions;
using SimpleIdentityServer.Module.Feed.Core.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Module.Feed.Core.Tests.Projects
{
    public class GetProjectActionFixture
    {
        private Mock<IProjectRepository> _projectRepositoryStub;
        private IGetProjectAction _getProjectAction;

        [Fact]
        public async Task WhenNullParametersArePassedThenExceptionsAreThrown()
        {
            // ARRANGE
            InitializeFakeObjects();
            
            // ACTS & ASSERTS
            await Assert.ThrowsAsync<ArgumentNullException>(() => _getProjectAction.Execute(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _getProjectAction.Execute(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _getProjectAction.Execute("id", null));
        }

        private void InitializeFakeObjects()
        {
            _projectRepositoryStub = new Mock<IProjectRepository>();
            _getProjectAction = new GetProjectAction(_projectRepositoryStub.Object);
        }
    }
}
