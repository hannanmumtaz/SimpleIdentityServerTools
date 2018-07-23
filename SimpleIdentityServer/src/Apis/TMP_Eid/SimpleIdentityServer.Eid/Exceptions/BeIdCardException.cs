using System;

namespace SimpleIdentityServer.Eid.Exceptions
{
    public class BeIdCardException : Exception
    {
        public BeIdCardException(string message) : base(message) { }
    }
}
