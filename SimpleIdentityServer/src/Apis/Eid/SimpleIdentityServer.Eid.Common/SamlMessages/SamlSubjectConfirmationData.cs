namespace SimpleIdentityServer.Eid.Common.SamlMessages
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
