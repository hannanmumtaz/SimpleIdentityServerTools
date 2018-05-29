namespace SimpleIdentityServer.Common.Saml.SamlMessages
{
    public class SamlAttributeDesignator
    {
        public SamlAttributeDesignator(string attributeName, string attributeNamespace)
        {
            AttributeName = attributeName;
            AttributeNamespace = attributeNamespace;
        }

        public string AttributeName { get; private set; }
        public string AttributeNamespace { get; private set; }
    }
}
