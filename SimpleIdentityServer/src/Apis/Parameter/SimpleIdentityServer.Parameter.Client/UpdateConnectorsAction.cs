﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Parameter.Common.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Parameter.Client
{
    public interface IUpdateConnectorsAction
    {
        Task<ErrorResponse> Execute(string baseUrl, IEnumerable<UpdateConnectorRequest> parameters, string accessToken = null);
    }

    internal sealed class UpdateConnectorsAction : IUpdateConnectorsAction
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UpdateConnectorsAction(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ErrorResponse> Execute(string baseUrl, IEnumerable<UpdateConnectorRequest> parameters, string accessToken = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var httpClient = _httpClientFactory.GetHttpClient();
            var serializedJson = JArray.FromObject(parameters).ToString();
            var body = new StringContent(serializedJson, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{baseUrl}/parameters/connectors"),
                Content = body
            };
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.Headers.Add("Authorization", $"Bearer {accessToken}");
            }

            var httpResponse = await httpClient.SendAsync(request).ConfigureAwait(false);
            var json = await httpResponse.Content.ReadAsStringAsync();
            try
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            catch(Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return new ErrorResponse
                    {
                        Error = "internal",
                        ErrorDescription = ex.Message
                    };
                }

                return JsonConvert.DeserializeObject<ErrorResponse>(json);
            }

            return null;
        }
    }
}