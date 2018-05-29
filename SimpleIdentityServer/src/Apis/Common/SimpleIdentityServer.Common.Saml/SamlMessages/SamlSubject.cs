namespace SimpleIdentityServer.Common.Saml.SamlMessages
{
    public class SamlSubject
    {
        public SamlSubject(SamlNameIdentifier nameIdentifier)
        {
            NameIdentifier = nameIdentifier;
        }

        public SamlSubject(SamlNameIdentifier nameIdentifier, SamlSubjectConfirmation subjectConfirmation)
        {
            NameIdentifier = nameIdentifier;
            SubjectConfirmation = subjectConfirmation;
        }

        public SamlNameIdentifier NameIdentifier { get; private set; }
        public SamlSubjectConfirmation SubjectConfirmation { get; private set; }
    }
}
