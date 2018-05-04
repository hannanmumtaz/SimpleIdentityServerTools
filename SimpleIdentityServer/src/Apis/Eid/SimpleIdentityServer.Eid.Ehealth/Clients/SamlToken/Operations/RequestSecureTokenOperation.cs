using SimpleIdentityServer.Eid.Common.Serializers;
using SimpleIdentityServer.Eid.Common.SoapMessages;
using SimpleIdentityServer.Eid.Ehealth.Exceptions;
using SimpleIdentityServer.Eid.Ehealth.Factories;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SimpleIdentityServer.Eid.Ehealth.Clients.SamlToken.Operations
{
    public interface IRequestSecureTokenOperation
    {
        Task<XmlNode> Execute(SoapEnvelope soapEnvelope, Uri uri);
        Task<XmlNode> Execute(string xml, Uri uri);
    }

    public class RequestSecureTokenOperation : IRequestSecureTokenOperation
    {
        private const string _soapAction = "urn:be:fgov:ehealth:sts:protocol:v1:RequestSecurityToken";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISoapMessageSerializer _soapMessageSerializer;

        public RequestSecureTokenOperation()
        {
            _httpClientFactory = new HttpClientFactory();
            _soapMessageSerializer = new SoapMessageSerializer();
        }

        public Task<XmlNode> Execute(SoapEnvelope soapEnvelope, Uri uri)
        {
            if (soapEnvelope == null)
            {
                throw new ArgumentNullException(nameof(soapEnvelope));
            }

            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            var xmlDocument = _soapMessageSerializer.Serialize(soapEnvelope);
            return Execute(xmlDocument.OuterXml, uri);
        }
        
        public async Task<XmlNode> Execute(string xml, Uri uri)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentNullException(nameof(xml));
            }

            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            var client = _httpClientFactory.GetHttpClient();
            var content = new StringContent(xml, Encoding.UTF8, "application/xml");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = content
            };
            request.Headers.Add(Constants.HttpHeaderParameters.SoapAction, _soapAction);
            var result = await client.SendAsync(request);
            result.EnsureSuccessStatusCode(); // TH : Throw a custom exception.
            var str = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            var document = new XmlDocument();
            document.LoadXml(str);
            var node = document.SelectSingleNode("Assertion");
            if (node == null)
            {
                throw new EhealthSecurityException(Constants.ErrorCodes.NoAssertion);
            }

            return node;
        }
    }
}
