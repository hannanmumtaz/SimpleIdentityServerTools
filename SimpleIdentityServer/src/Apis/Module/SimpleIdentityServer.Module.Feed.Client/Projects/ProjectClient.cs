using SimpleIdentityServer.Module.Feed.Common.Responses;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Client.Projects
{
    public interface IProjectClient
    {
        Task<IEnumerable<string>> Get(string url);
        Task<IEnumerable<ProjectResponse>> Get(string url, string projectName);
        Task<ProjectResponse> Get(string url, string projectName, string version);
        Task<Stream> Download(string url, string projectName, string version);
    }

    internal sealed class ProjectClient : IProjectClient
    {
        private readonly IConfigurationClient _configurationClient;
        private readonly IGetProjectOperation _getProjectOperation;
        private readonly IDownloadProjectConfiguration _downloadProjectConfiguration;

        public ProjectClient(IConfigurationClient configurationClient, IGetProjectOperation getProjectOperation, IDownloadProjectConfiguration downloadProjectConfiguration)
        {
            _configurationClient = configurationClient;
            _getProjectOperation = getProjectOperation;
            _downloadProjectConfiguration = downloadProjectConfiguration;
        }

        public async Task<IEnumerable<string>> Get(string url)
        {
            var configuration = await _configurationClient.GetConfiguration(url);
            return await _getProjectOperation.Execute(configuration.ProjectsEndpoint);
        }

        public async Task<IEnumerable<ProjectResponse>> Get(string url, string projectName)
        {
            var configuration = await _configurationClient.GetConfiguration(url);
            return await _getProjectOperation.Execute(configuration.ProjectsEndpoint, projectName);
        }

        public async Task<ProjectResponse> Get(string url, string projectName, string version)
        {
            var configuration = await _configurationClient.GetConfiguration(url);
            return await _getProjectOperation.Execute(configuration.ProjectsEndpoint, projectName, version);
        }

        public async Task<Stream> Download(string url, string projectName, string version)
        {
            var configuration = await _configurationClient.GetConfiguration(url);
            return await _downloadProjectConfiguration.Execute(configuration.ProjectsEndpoint, projectName, version);
        }
    }
}