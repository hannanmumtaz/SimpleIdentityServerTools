namespace SimpleIdentityServer.Eid.Ehealth.Exceptions
{
    public class EhealthSecurityException : EhealthException
    {
        public EhealthSecurityException(string code) : base(code) { }
    }
}
