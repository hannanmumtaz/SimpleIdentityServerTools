using SimpleIdentityServer.Common.Client.Factories;
using SimpleIdentityServer.Common.Saml.Serializers;
using SimpleIdentityServer.Common.Saml.SoapMessages;
using SimpleIdentityServer.Rms.Client.DTOs.Responses;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SimpleIdentityServer.Rms.Client.Licensing.TemplateDistribution
{
    public interface ILicensingAcquireTemplateInformationOperation
    {
        Task<AcquireTemplatesResponse> Execute(string baseUrl);
    }

    internal class LicensingAcquireTemplateInformationOperation : ILicensingAcquireTemplateInformationOperation
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISoapMessageSerializer _soapMessageSerializer;

        public LicensingAcquireTemplateInformationOperation(IHttpClientFactory httpClientFactory, ISoapMessageSerializer soapMessageSerializer)
        {
            _httpClientFactory = httpClientFactory;
            _soapMessageSerializer = soapMessageSerializer;

        }

        public async Task<AcquireTemplatesResponse> Execute(string baseUrl)
        {
            var httpClient = _httpClientFactory.GetHttpClient();
            var soapHeader = new SoapHeader(null);
            var nodeDocument = new XmlDocument();
            var actionNode = nodeDocument.CreateNode(XmlNodeType.Element, "AcquireTemplateInformation", "http://microsoft.com/DRM/TemplateDistributionService");
            var soapBody = new SoapBody(actionNode);
            var soapEnvelope = new SoapEnvelope(soapHeader, soapBody);
            var xmlDocumentRequest = _soapMessageSerializer.Serialize(soapEnvelope);
            var content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(xmlDocumentRequest.OuterXml)));
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{baseUrl}//_wmcs/licensing/templatedistribution.asmx"),
                Content = content
            };
            request.Content.Headers.Add("Content-Type", "text/xml");
            request.Content.Headers.Add("SOAPAction", "http://microsoft.com/DRM/TemplateDistributionService/AcquireTemplateInformation");
            var httpResponse = await httpClient.SendAsync(request).ConfigureAwait(false);
            var resp = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            return _soapMessageSerializer.Deserialize<AcquireTemplatesResponse>(resp, "lic", "http://microsoft.com/DRM/TemplateDistributionService", "AcquireTemplateInformationResult");
        }
    }
}
