using System;

namespace SimpleIdentityServer.Common.Saml.SamlMessages
{
    public class SamlAssertion
    {
        public SamlAssertion(string assertionId, DateTime issueInstant, string issuer, SamlConditions samlConditions, SamlAttributeStatement samlAttributeStatement)
        {
            AssertionId = assertionId;
            IssueInstant = issueInstant;
            Issuer = issuer;
            MajorVersion = 1;
            MinorVersion = 1;
            Conditions = samlConditions;
            AttributeStatement = samlAttributeStatement;
        }
        
        public string AssertionId { get; private set; }
        public DateTime IssueInstant { get; private set; } 
        public string Issuer { get; private set; }
        public int MajorVersion { get; private set; }
        public int MinorVersion { get; private set; }
        public SamlConditions Conditions { get; private set; }
        public SamlAttributeStatement AttributeStatement { get; private set; }
    }
}
