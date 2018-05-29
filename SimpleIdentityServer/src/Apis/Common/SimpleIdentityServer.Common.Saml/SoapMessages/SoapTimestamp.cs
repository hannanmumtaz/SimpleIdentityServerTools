using System;

namespace SimpleIdentityServer.Common.Saml.SoapMessages
{
    public class SoapTimestamp
    {
        public SoapTimestamp(DateTime created, string id)
        {
            Created = created;
            Expires = created.AddMinutes(1);
            Id = id;
        }

        public SoapTimestamp(DateTime created, DateTime expires, string id)
        {
            Created = created;
            Expires = expires;
            Id = id;
        }

        public DateTime Created { get; private set; }
        public DateTime Expires { get; private set; }
        public string Id { get; private set; }
    }
}
