namespace SimpleIdentityServer.Eid.Common.SoapMessages
{
    public class SoapReference
    {
        public SoapReference(string id, string value)
        {
            Id = id;
            DigestValue = value;
            TransformAlgorithm = "http://www.w3.org/2001/10/xml-exc-c14n#";
            DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
        }

        public string Id { get; set; }
        public string DigestValue { get; set; }
        public string TransformAlgorithm { get; set; }
        public string DigestMethod { get; set; }
    }
}
