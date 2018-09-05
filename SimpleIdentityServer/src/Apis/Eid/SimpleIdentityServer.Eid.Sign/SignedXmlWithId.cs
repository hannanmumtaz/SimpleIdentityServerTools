using System.Security.Cryptography.Xml;
using System.Xml;

namespace SimpleIdentityServer.Eid.Sign
{
    public class SignedXmlWithId : SignedXml
    {
        public const string XmlSoapEnvelopeUrl = "http://schemas.xmlsoap.org/soap/envelope/";
        public const string XmlSoapSecurityUrl = "http://schemas.xmlsoap.org/soap/security/2000-12";
        public const string XmlOasisWSSSecurityUtilUrl = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        public const string XmlOasisWSSSecurityExtUrl = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

        public SignedXmlWithId(XmlDocument xml) : base(xml)
        {
        }

        public SignedXmlWithId(XmlElement xmlElement) : base(xmlElement)
        {
        }

        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            XmlElement idElem = base.GetIdElement(doc, id);
            if (idElem == null)
            {
                XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                nsManager.AddNamespace("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

                idElem = doc.SelectSingleNode("//*[@wsu:Id=\"" + id + "\"]", nsManager) as XmlElement;
            }

            return idElem;
        }
    }
}
