namespace SimpleIdentityServer.Common.Saml.SoapMessages
{
    public class SoapKeyInfo
    {
        public SoapKeyInfo(string id, string securityTokenReferenceId, string referenceId)
        {
            Id = id;
            SecurityTokenReferenceId = securityTokenReferenceId;
            ReferenceId = referenceId;
        }

        public string Id { get; private set; }
        public string SecurityTokenReferenceId { get; private set; }
        public string ReferenceId { get; private set; }
    }
}
