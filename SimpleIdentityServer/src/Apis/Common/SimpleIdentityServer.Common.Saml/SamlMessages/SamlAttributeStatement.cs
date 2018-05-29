using System.Collections.Generic;

namespace SimpleIdentityServer.Common.Saml.SamlMessages
{
    public class SamlAttributeStatement
    {
        public SamlAttributeStatement(SamlSubject subject, IEnumerable<SamlAttribute> attributes)
        {
            Subject = subject;
            Attributes = attributes;
        }

        public SamlSubject Subject { get; private set; }
        public IEnumerable<SamlAttribute> Attributes { get; private set; }
    }
}
