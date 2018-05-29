namespace SimpleIdentityServer.Common.Saml.SamlMessages
{
    public class SamlAttribute
    {
        public SamlAttribute(string name, string namespaceValue, string value)
        {
            Name = name;
            Namespace = namespaceValue;
            Value = value;
        }

        public string Name { get; private set; }
        public string Namespace { get; set; }
        public string Value { get; set; }
    }
}
