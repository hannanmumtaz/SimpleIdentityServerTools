namespace SimpleIdentityServer.Eid.Common.SamlMessages
{
    public class SamlNameIdentifier
    {
        public SamlNameIdentifier(string format, string nameQualifier, string value)
        {
            Format = format;
            NameQualifier = nameQualifier;
            Value = value;
        }

        public string Format { get; private set; }
        public string NameQualifier { get; private set; }
        public string Value { get; private set; }
    }
}
