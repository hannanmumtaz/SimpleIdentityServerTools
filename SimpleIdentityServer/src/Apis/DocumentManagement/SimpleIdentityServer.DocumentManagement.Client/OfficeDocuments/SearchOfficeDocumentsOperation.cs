﻿using Newtonsoft.Json;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.DocumentManagement.Client.Responses;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments
{
    public interface ISearchOfficeDocumentsOperation
    {
        Task<GetSearchOfficeDocumentsResponse> Execute(SearchOfficeDocumentRequest request, string url, string accessToken);
    }

    internal sealed class SearchOfficeDocumentsOperation : ISearchOfficeDocumentsOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SearchOfficeDocumentsOperation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetSearchOfficeDocumentsResponse> Execute(SearchOfficeDocumentRequest request, string url, string accessToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var serializedPostPermission = JsonConvert.SerializeObject(request);
            var body = new StringContent(serializedPostPermission, Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = body,
                RequestUri = new Uri(url + "/.search")
            };
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                httpRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            }
            var httpResponse = await httpClient.SendAsync(httpRequest).ConfigureAwait(false);
            var json = await httpResponse.Content.ReadAsStringAsync();
            try
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return new GetSearchOfficeDocumentsResponse
                {
                    HttpStatus = httpResponse.StatusCode,
                    ContainsError = true,
                    Error = JsonConvert.DeserializeObject<ErrorResponse>(json)
                };
            }

            return new GetSearchOfficeDocumentsResponse
            {
                Content = JsonConvert.DeserializeObject<SearchOfficeDocumentsResponse>(json)
            };
        } 
    }
}