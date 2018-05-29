using System.Collections.Generic;

namespace SimpleIdentityServer.Common.Saml.SamlMessages
{
    public class SamlAttributeQuery
    {
        public SamlAttributeQuery(SamlSubject subject, IEnumerable<SamlAttributeDesignator> designators)
        {
            Subject = subject;
            Designators = designators;
        }

        public SamlSubject Subject { get; private set; }
        public IEnumerable<SamlAttributeDesignator> Designators { get; private set; }
    }
}
