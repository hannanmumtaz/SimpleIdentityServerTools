using Moq;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Module.Feed.Client.Projects;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Module.Feed.Client.Tests
{
    public class ProjectsClientFixture : IClassFixture<TestModuleFeedFixture>
    {
        private Mock<IHttpClientFactory> _httpClientFactoryStub;
        private IProjectClient _projectClient;
        private readonly TestModuleFeedFixture _server;

        public ProjectsClientFixture(TestModuleFeedFixture server)
        {
            _server = server;
        }

        [Fact]
        public async Task WhenGetAllProjectsThenNamesAreReturned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var projectNames = await _projectClient.Get("http://localhost:5000/configuration");

            // ASSERT
            Assert.NotNull(projectNames);
            Assert.True(projectNames.Contains("OpenIdProvider"));
        }

        [Fact]
        public async Task WhenGetProjectThenInformationsAreReturned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var projects = await _projectClient.Get("http://localhost:5000/configuration", "OpenIdProvider");

            // ASSERT
            Assert.NotNull(projects);
            Assert.Equal(1, projects.Count());
        }

        [Fact]
        public async Task WhenGetProjectVersionThenInformationsAreReturned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var project = await _projectClient.Get("http://localhost:5000/configuration", "OpenIdProvider", "3.0.0-rc7");

            // ASSERT
            Assert.NotNull(project);
            Assert.Equal("3.0.0-rc7", project.Version);
            Assert.Equal("OpenIdProvider", project.ProjectName);
            Assert.Equal(4, project.Units.Count());
        }

        [Fact]
        public async Task WhenDownloadProjectInformationThenConfigFileIsReturned()
        {
            // ARRANGE
            InitializeFakeObjects();
            _httpClientFactoryStub.Setup(h => h.GetHttpClient()).Returns(_server.Client);

            // ACT
            var stream = await _projectClient.Download("http://localhost:5000/configuration", "OpenIdProvider", "3.0.0-rc7");

            // ASSERT
            Assert.NotNull(stream);
        }

        private void InitializeFakeObjects()
        {
            _httpClientFactoryStub = new Mock<IHttpClientFactory>();
            var configurationClient = new ConfigurationClient(_httpClientFactoryStub.Object);
            _projectClient = new ProjectClient(configurationClient, new GetProjectOperation(_httpClientFactoryStub.Object),
                new DownloadProjectConfiguration(_httpClientFactoryStub.Object));
        }
    }
}
