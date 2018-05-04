using System;

namespace SimpleIdentityServer.Eid.Common.SamlMessages
{
    public class SamlConditions
    {
        public SamlConditions(DateTime issueInstant)
        {
            NotBefore = issueInstant;
            NotOnOrAfter = issueInstant.AddDays(1);
        }

        public DateTime NotBefore { get; private set; }
        public DateTime NotOnOrAfter { get; private set; }
    }
}
