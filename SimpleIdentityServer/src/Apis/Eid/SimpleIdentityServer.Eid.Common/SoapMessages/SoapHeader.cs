namespace SimpleIdentityServer.Eid.Common.SoapMessages
{
    public class SoapHeader
    {
        public SoapHeader(SoapSecurity security)
        {
            Security = security;
        }

        public SoapSecurity Security { get; private set; }
    }
}
