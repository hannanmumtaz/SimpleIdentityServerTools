namespace SimpleIdentityServer.Common.Saml.SamlMessages
{
    public class SamlSubjectConfirmationData
    {
        public SamlSubjectConfirmationData(SamlAssertion assertion)
        {
            Assertion = assertion;
        }

        public SamlAssertion Assertion { get; private set; }
    }
}
