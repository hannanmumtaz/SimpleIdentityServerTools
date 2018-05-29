using SimpleIdentityServer.Common.Saml.SamlMessages;
using System.Xml;

namespace SimpleIdentityServer.Common.Saml.SoapMessages
{
    public class SoapBody
    {
        public SoapBody(XmlNode content)
        {
            Content = content;
        }

        public SoapBody(SamlRequest request, string id)
        {
            Request = request;
            Id = id;
        }

        public XmlNode Content { get; private set; }
        public string Id { get; private set; }
        public SamlRequest Request { get; private set; }
    }
}
