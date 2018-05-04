using System;

namespace SimpleIdentityServer.Eid.Common.SamlMessages
{
    public class SamlRequest
    {
        public SamlRequest(string id, SamlAttributeQuery samlAttributeQuery)
        {
            Id = id;
            SamlAttributeQuery = samlAttributeQuery;
            IssueInstant = DateTime.Now;
            MinorVersion = 1;
            MajorVersion = 1;
        }

        public DateTime IssueInstant { get; private set; }
        public int MinorVersion { get; private set; }
        public int MajorVersion { get; private set; }
        public string Id { get; private set; }
        public SamlAttributeQuery SamlAttributeQuery { get; private set; }
    }
}
