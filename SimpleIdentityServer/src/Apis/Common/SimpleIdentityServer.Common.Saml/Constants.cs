using System.Collections.Generic;

namespace SimpleIdentityServer.Common.Saml
{
    public static class Constants
    {
        public static Dictionary<string, string> MappingRoleToNihiiNamespace = new Dictionary<string, string>
        {
            { "doctor", "urn:be:fgov:person:ssin:ehealth:1.0:doctor:nihii11" },
            { "dentist", "urn:be:fgov:person:ssin:ehealth:1.0:nihii:dentist:nihii11" },
            { "physiotherapist", "urn:be:fgov:person:ssin:ehealth:1.0:nihii:physiotherapist:nihii11" },
            { "nurse", "urn:be:fgov:person:ssin:ehealth:1.0:nihii:nurse:nihii11" }
        };

        public static Dictionary<string, string> MappingRoleToPracticionnerNamespace = new Dictionary<string, string>
        {
            { "doctor", "urn:be:fgov:person:ssin:ehealth:1.0:nihii:doctor:generalist:boolean" }
        };

        public static class XmlNamespaces
        {
            public const string SoapEnvelope = "http://schemas.xmlsoap.org/soap/envelope/";
            public const string Samlp = "urn:oasis:names:tc:SAML:1.0:protocol";
            public const string Saml = "urn:oasis:names:tc:SAML:1.0:assertion";
            public const string Wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
            public const string Ds = "http://www.w3.org/2000/09/xmldsig#";
            public const string Ns4 = "urn:oasis:names:tc:SAML:1.0:protocol";
            public const string Wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            public const string EncodingType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";
            public const string ValueType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";
            public const string Sha1 = "http://www.w3.org/2000/09/xmldsig#sha1";
        }

        public static class XmlRootNames
        {
            public const string SoapHeader = "Header";
            public const string SoapEnvelope = "Envelope";
            public const string SoapBody = "Body";
            public const string SamlRequest = "Request";
            public const string SamlAttributeQuery = "AttributeQuery";
            public const string SamlSubject = "Subject";
            public const string SamlNameIdentifier = "NameIdentifier";
            public const string SamlSubjectConfirmation = "SubjectConfirmation";
            public const string SamlConfirmationMethod = "ConfirmationMethod";
            public const string SamlSubjectConfirmationData = "SubjectConfirmationData";
            public const string SamlAssertion = "Assertion";
            public const string SamlConditions = "Conditions";
            public const string SamlAttributeStatement = "AttributeStatement";
            public const string SamlAttribute = "Attribute";
            public const string SamlAttributeDesignator = "AttributeDesignator";
            public const string SamlAttributeValue = "AttributeValue";
            public const string KeyInfo = "KeyInfo";
            public const string X509Data = "X509Data";
            public const string X509Certificate = "X509Certificate";
            public const string Security = "Security";
            public const string Timestamp = "Timestamp";
            public const string Created = "Created";
            public const string Expires = "Expires";
            public const string BinarySecurityToken = "BinarySecurityToken";
            public const string SecurityTokenReference = "SecurityTokenReference";
            public const string Reference = "Reference";
            public const string Signature = "Signature";
            public const string SignedInfo = "SignedInfo";
            public const string CanonicalizationMethod = "CanonicalizationMethod";
            public const string SignatureMethod = "SignatureMethod";
            public const string Transforms = "Transforms";
            public const string Transform = "Transform";
            public const string DigestMethod = "DigestMethod";
            public const string DigestValue = "DigestValue";
            public const string SignatureValue = "SignatureValue";

        }

        public static class XmlPrefixes
        {
            public const string SoapEnv = "SOAP-ENV";
            public const string Wsu = "wsu";
            public const string Samlp = "samlp";
            public const string Ds = "ds";
            public const string Ns4 = "ns4";
            public const string Saml = "saml";
            public const string Wsse = "wsse";
        }

        public static class XmlAttributeNames
        {
            public const string SamlIssueInstant = "IssueInstant";
            public const string SamlIssuer = "Issuer";
            public const string SamlMajorVersion = "MajorVersion";
            public const string SamlMinorVersion = "MinorVersion";
            public const string SamlRequestId = "RequestID";
            public const string SamlNameIdentifierFormat = "Format";
            public const string SamlNameQualifier = "NameQualifier";
            public const string SamlAttributeName = "AttributeName";
            public const string SamlAttributeNamespace = "AttributeNamespace";
            public const string SamlAssertionId = "AssertionID";
            public const string SamlNotBefore = "NotBefore";
            public const string SamlNotOnOrAfter = "NotOnOrAfter";
            public const string EncodingType = "EncodingType";
            public const string ValueType = "ValueType";
            public const string Uri = "URI";
            public const string Algorithm = "Algorithm";
        }
    }
}
