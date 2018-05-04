using System.Collections.Generic;

namespace SimpleIdentityServer.Eid.Common.SoapMessages
{
    public class SoapSignature
    {
        public SoapSignature(string id, string signatureValue, SoapKeyInfo keyInfo)
        {
            Id = id;
            KeyInfo = keyInfo;
            SignatureValue = signatureValue;
            References = new List<SoapReference>();
            CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
            SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
        }

        public string Id { get; private set; }
        public SoapKeyInfo KeyInfo { get; private set; }
        public string SignatureValue { get; private set; }
        public ICollection<SoapReference> References { get; private set; }
        public string CanonicalizationMethod { get; private set; }
        public string SignatureMethod { get; private set; }
    }
}
