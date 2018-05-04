using System.Security.Cryptography.X509Certificates;

namespace SimpleIdentityServer.Eid.Common.SamlMessages
{
    public class SamlSubjectConfirmation
    {
        public SamlSubjectConfirmation(string method, X509Certificate certificate, SamlSubjectConfirmationData subjectConfirmationData)
        {
            Method = method;
            SubjectConfirmationData = subjectConfirmationData;
            Certificate = certificate;
        }

        public string Method { get; private set; }
        public SamlSubjectConfirmationData SubjectConfirmationData { get; private set; }
        public X509Certificate Certificate { get; private set; }
    }
}
