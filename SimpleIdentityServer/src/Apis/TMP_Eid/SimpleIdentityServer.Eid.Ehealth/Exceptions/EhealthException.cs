using System;

namespace SimpleIdentityServer.Eid.Ehealth.Exceptions
{
    public class EhealthException : Exception
    {
        public EhealthException(string code) : base(code) { }
    }
}
