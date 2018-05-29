using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace SimpleIdentityServer.Common.Saml.SoapMessages
{
    public class SoapSecurity
    {
        public SoapSecurity(XmlNode assertion)
        {
            Assertion = assertion;
        }

        public SoapSecurity(DateTime created, string idTimeStamp, string idBinarySecurityToken, X509Certificate certificate)
        {
            Timestamp = new SoapTimestamp(created, idTimeStamp);
            Certificate = certificate;
            IdBinarySecurityToken = idBinarySecurityToken;
        }

        public XmlNode Assertion { get; private set; }
        public SoapTimestamp Timestamp { get; private set; }
        public X509Certificate Certificate { get; private set; }
        public string IdBinarySecurityToken { get; private set; }
        public SoapSignature Signature { get; set; }
    }
}
