using System.Collections.Generic;

namespace SimpleIdentityServer.Eid.Common.SamlMessages
{
    public class SamlKeyInfo
    {
        public SamlKeyInfo(IEnumerable<byte> payload)
        {
            Payload = payload;
        }
        
        public IEnumerable<byte> Payload { get; set; }
    }
}
