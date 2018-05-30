using SimpleIdentityServer.Common.Client.Factories;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Client.Projects
{
    public interface IDownloadProjectConfiguration
    {
        Task<Stream> Execute(string url, string projectName, string version);
    }

    internal sealed class DownloadProjectConfiguration : IDownloadProjectConfiguration
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGetProjectOperation _getProjectOperation;
        private readonly IDownloadProjectConfiguration _downloadProjectConfiguration;

        public DownloadProjectConfiguration(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Stream> Execute(string url, string projectName, string version)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentNullException(nameof(projectName));
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentNullException(nameof(version));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{url}/{projectName}/{version}/download")
            };
            var httpResponse = await httpClient.SendAsync(request).ConfigureAwait(false);
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }
    }
}
