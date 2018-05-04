using SimpleIdentityServer.Eid.Common.SoapMessages;
using SimpleIdentityServer.Eid.Ehealth.Clients.SamlToken.Operations;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace SimpleIdentityServer.Eid.Ehealth.Clients.SamlToken
{
    public interface ISamlTokenClient
    {
        Task<XmlNode> RequestSecureToken(SoapEnvelope soapEnvelope, Uri uri);
        Task<XmlNode> RequestSecureToken(string xml, Uri uri);
    }

    public class SamlTokenClient : ISamlTokenClient
    {
        private readonly IRequestSecureTokenOperation _requestSecureTokenOperation;

        public SamlTokenClient()
        {
            _requestSecureTokenOperation = new RequestSecureTokenOperation();
        }

        public Task<XmlNode> RequestSecureToken(SoapEnvelope soapEnvelope, Uri uri)
        {
            return _requestSecureTokenOperation.Execute(soapEnvelope, uri);
        }

        public Task<XmlNode> RequestSecureToken(string xml, Uri uri)
        {
            return _requestSecureTokenOperation.Execute(xml, uri);
        }
    }
}
