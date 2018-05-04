using SimpleIdentityServer.Eid.Common.SamlMessages;
using SimpleIdentityServer.Eid.Common.SoapMessages;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleIdentityServer.Eid.Common.Serializers
{
    public interface ISoapMessageSerializer
    {
        T Deserialize<T>(string xml, string xmlElementName);
        T Deserialize<T>(string xml, string prefix, string nameSpace, string xmlElementName);
        SoapTimestamp ExtractSoapTimestamp(XmlDocument xmlNode);
        XmlDocument SerializeBody<T>(T request) where T : RequestType;
        XmlDocument Serialize<T>(T request) where T : RequestType;
        XmlDocument Serialize<T>(T request, XmlElement assertionNode) where T : RequestType;
        XmlDocument Serialize(SoapEnvelope soapEnvelope);
    }

    public class SoapMessageSerializer : ISoapMessageSerializer
    {
        public T Deserialize<T>(string xml, string xmlElementName)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentNullException(nameof(xml));
            }

            var resultDoc = new XmlDocument();
            resultDoc.LoadXml(xml);
            var namespaceManager = new XmlNamespaceManager(resultDoc.NameTable);
            var node = resultDoc.SelectSingleNode($"//{xmlElementName}");
            var outerXml = node.OuterXml;
            var deserializer = new XmlSerializer(typeof(T));
            var settings = new XmlReaderSettings();
            using (var reader = new StringReader(xml))
            {
                return (T)deserializer.Deserialize(reader);
            }
        }

        public T Deserialize<T>(string xml, string prefix, string nameSpace, string xmlElementName)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentNullException(nameof(xml));
            }

            var resultDoc = new XmlDocument();
            resultDoc.LoadXml(xml);
            var namespaceManager = new XmlNamespaceManager(resultDoc.NameTable);
            namespaceManager.AddNamespace(prefix, nameSpace);
            var node = resultDoc.SelectSingleNode($"//{prefix}:{xmlElementName}", namespaceManager);
            var outerXml = node.OuterXml;
            var deserializer = new XmlSerializer(typeof(T));
            var settings = new XmlReaderSettings();
            using (var reader = new StringReader(xml))
            {
                return (T)deserializer.Deserialize(reader);
            }
        }

        public SoapTimestamp ExtractSoapTimestamp(XmlDocument node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            
            var ns = new XmlNamespaceManager(node.NameTable);
            ns.AddNamespace("assert", "urn:oasis:names:tc:SAML:1.0:assertion");
            var timeStampNode = node.SelectSingleNode("//assert:Conditions", ns);
            if (timeStampNode == null)
            {
                return null;
            }

            var createdNode = timeStampNode.Attributes["NotBefore"];
            var expiresNode = timeStampNode.Attributes["NotOnOrAfter"];
            if (createdNode == null || expiresNode == null)
            {
                return null;
            }

            DateTime createdDateTime, expiresDateTime;
            if (!DateTime.TryParse(createdNode.InnerText, out createdDateTime) || !DateTime.TryParse(expiresNode.InnerText, out expiresDateTime))
            {
                return null;
            }

            // TH : Extract the identifier.
            return new SoapTimestamp(createdDateTime, expiresDateTime, null);
        }

        public XmlDocument SerializeBody<T>(T request) where T : RequestType
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            
            var serialzer = new XmlSerializer(typeof(T));
            string xml;
            using (var strW = new StringWriter())
            {
                using (var writer = XmlWriter.Create(strW))
                {
                    serialzer.Serialize(writer, request);
                    xml = strW.ToString();
                }
            }

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        public XmlDocument Serialize<T>(T request) where T : RequestType
        {
            return Serialize(request, null);
        }

        public XmlDocument Serialize<T>(T request, XmlElement assertionNode) where T : RequestType
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var serialzer = new XmlSerializer(typeof(T));
            string xml;
            using (var strW = new StringWriter())
            {
                using (var writer = XmlWriter.Create(strW))
                {
                    serialzer.Serialize(writer, request);
                    xml = strW.ToString();
                }
            }

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var soapBody = new SoapBody(doc.DocumentElement);
            SoapSecurity soapSecurity = null;
            if (assertionNode != null)
            {
                soapSecurity = new SoapSecurity(assertionNode);
            }

            var soapHeader = new SoapHeader(soapSecurity);
            var soapEnvelope = new SoapEnvelope(soapHeader, soapBody);
            var serializer = new SoapMessageSerializer();
            return serializer.Serialize(soapEnvelope);
        }

        public XmlDocument Serialize(SoapEnvelope soapEnvelope)
        {
            if (soapEnvelope == null)
            {
                throw new ArgumentNullException(nameof(soapEnvelope));
            }

            var document = new XmlDocument();
            var soapEnvNode = document.CreateNode(XmlNodeType.Element, Constants.XmlPrefixes.SoapEnv, Constants.XmlRootNames.SoapEnvelope, Constants.XmlNamespaces.SoapEnvelope);
            Serialize(soapEnvelope.Header, document, soapEnvNode);
            Serialize(soapEnvelope.Body, document, soapEnvNode);
            document.AppendChild(soapEnvNode);
            return document;
        }

        private static void Serialize(SoapHeader soapHeader, XmlDocument document, XmlNode root)
        {
            if (soapHeader == null)
            {
                throw new ArgumentNullException(nameof(soapHeader));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var headerNode = document.CreateElement(Constants.XmlPrefixes.SoapEnv, Constants.XmlRootNames.SoapHeader, Constants.XmlNamespaces.SoapEnvelope);
            if (soapHeader.Security != null)
            {
                Serialize(soapHeader.Security, document, headerNode);
            }

            root.AppendChild(headerNode);
        }

        private static void Serialize(SoapSecurity soapSecurity, XmlDocument document, XmlNode root)
        {
            if (soapSecurity == null)
            {
                throw new ArgumentNullException(nameof(soapSecurity));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var soapSecurityNode = document.CreateElement(Constants.XmlPrefixes.Wsse, Constants.XmlRootNames.Security, Constants.XmlNamespaces.Wsse);
            if (soapSecurity.Assertion == null)
            {
                var timeStampNode = document.CreateElement(Constants.XmlPrefixes.Wsu, Constants.XmlRootNames.Timestamp, Constants.XmlNamespaces.Wsu);
                var createdNode = document.CreateElement(Constants.XmlPrefixes.Wsu, Constants.XmlRootNames.Created, Constants.XmlNamespaces.Wsu);
                var expiresNode = document.CreateElement(Constants.XmlPrefixes.Wsu, Constants.XmlRootNames.Expires, Constants.XmlNamespaces.Wsu);
                var binarySecurityTokenNode = document.CreateElement(Constants.XmlPrefixes.Wsse, Constants.XmlRootNames.BinarySecurityToken, Constants.XmlNamespaces.Wsse);

                soapSecurityNode.SetAttribute($"xmlns:{Constants.XmlPrefixes.Wsu}", Constants.XmlNamespaces.Wsu);
                soapSecurityNode.SetAttribute("SOAP-ENV:mustUnderstand", "1");
                timeStampNode.SetAttribute("wsu:Id", soapSecurity.Timestamp.Id);
                createdNode.InnerText = ConvertToUtcTime(soapSecurity.Timestamp.Created);
                expiresNode.InnerText = ConvertToUtcTime(soapSecurity.Timestamp.Expires);
                binarySecurityTokenNode.SetAttribute(Constants.XmlAttributeNames.EncodingType, Constants.XmlNamespaces.EncodingType);
                binarySecurityTokenNode.SetAttribute(Constants.XmlAttributeNames.ValueType, Constants.XmlNamespaces.ValueType);
                binarySecurityTokenNode.SetAttribute("wsu:Id", soapSecurity.IdBinarySecurityToken);
                var b64Cert = Convert.ToBase64String(soapSecurity.Certificate.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Cert));
                binarySecurityTokenNode.InnerText = b64Cert;

                timeStampNode.AppendChild(createdNode);
                timeStampNode.AppendChild(expiresNode);
                soapSecurityNode.AppendChild(timeStampNode);
                soapSecurityNode.AppendChild(binarySecurityTokenNode);
                if (soapSecurity.Signature != null)
                {
                    Serialize(soapSecurity.Signature, document, soapSecurityNode);
                }
            }
            else
            {
                var nodeCopy = document.ImportNode(soapSecurity.Assertion, true);
                soapSecurityNode.AppendChild(nodeCopy);
            }

            root.AppendChild(soapSecurityNode);
        }

        private static void Serialize(SoapSignature soapSignature, XmlDocument document, XmlNode root)
        {
            if (soapSignature == null)
            {
                throw new ArgumentNullException(nameof(soapSignature));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }
            
            var signatureNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.Signature, Constants.XmlNamespaces.Ds);
            var signedInfoNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.SignedInfo, Constants.XmlNamespaces.Ds);
            var canonicalizationMethodNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.CanonicalizationMethod, Constants.XmlNamespaces.Ds);
            var signatureMethodNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.SignatureMethod, Constants.XmlNamespaces.Ds);
            var signatureValueNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.SignatureValue, Constants.XmlNamespaces.Ds);

            signatureNode.SetAttribute($"Id", soapSignature.Id);
            canonicalizationMethodNode.SetAttribute(Constants.XmlAttributeNames.Algorithm, soapSignature.CanonicalizationMethod);
            signatureMethodNode.SetAttribute(Constants.XmlAttributeNames.Algorithm, soapSignature.SignatureMethod);
            signatureValueNode.InnerText = soapSignature.SignatureValue;

            signedInfoNode.AppendChild(canonicalizationMethodNode);
            signedInfoNode.AppendChild(signatureMethodNode);
            if (soapSignature.References != null)
            {
                foreach(var reference in soapSignature.References)
                {
                    Serialize(reference, document, signedInfoNode);
                }
            }

            signatureNode.AppendChild(signedInfoNode);
            signatureNode.AppendChild(signatureValueNode);
            Serialize(soapSignature.KeyInfo, document, signatureNode);
            root.AppendChild(signatureNode);
        }

        private static void Serialize(SoapReference reference, XmlDocument document, XmlNode root)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }
            
            var referenceNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.Reference, Constants.XmlNamespaces.Ds);
            var transformsNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.Transforms, Constants.XmlNamespaces.Ds);
            var transformNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.Transform, Constants.XmlNamespaces.Ds);
            var digestMethodNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.DigestMethod, Constants.XmlNamespaces.Ds);
            var digestValueNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.DigestValue, Constants.XmlNamespaces.Ds);

            referenceNode.SetAttribute(Constants.XmlAttributeNames.Uri, reference.Id);
            transformNode.SetAttribute(Constants.XmlAttributeNames.Algorithm, reference.TransformAlgorithm);
            digestMethodNode.SetAttribute(Constants.XmlAttributeNames.Algorithm, reference.DigestMethod);
            digestValueNode.InnerText = reference.DigestValue;

            transformsNode.AppendChild(transformNode);
            referenceNode.AppendChild(transformsNode);
            referenceNode.AppendChild(digestMethodNode);
            referenceNode.AppendChild(digestValueNode);
            root.AppendChild(referenceNode);
        }
        
        private static void Serialize(SoapKeyInfo keyInfo, XmlDocument document, XmlNode root)
        {
            if (keyInfo == null)
            {
                throw new ArgumentNullException(nameof(keyInfo));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }
            
            var keyInfoNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.KeyInfo, Constants.XmlNamespaces.Ds);
            var securityTokenReferenceNode = document.CreateElement(Constants.XmlPrefixes.Wsse, Constants.XmlRootNames.SecurityTokenReference, Constants.XmlNamespaces.Wsse);
            var referenceNode = document.CreateElement(Constants.XmlPrefixes.Wsse, Constants.XmlRootNames.Reference, Constants.XmlNamespaces.Wsse);

            referenceNode.SetAttribute(Constants.XmlAttributeNames.Uri, keyInfo.ReferenceId);
            referenceNode.SetAttribute(Constants.XmlAttributeNames.ValueType, Constants.XmlNamespaces.ValueType);
            securityTokenReferenceNode.SetAttribute($"{Constants.XmlPrefixes.Wsse}:Id", keyInfo.SecurityTokenReferenceId);
            keyInfoNode.SetAttribute($"{Constants.XmlPrefixes.Ds}:Id", keyInfo.Id);

            securityTokenReferenceNode.AppendChild(referenceNode);
            keyInfoNode.AppendChild(securityTokenReferenceNode);

            root.AppendChild(keyInfoNode);
        }

        private static void Serialize(SoapBody soapBody, XmlDocument document, XmlNode root)
        {
            if (soapBody == null)
            {
                throw new ArgumentNullException(nameof(soapBody));
            }
            
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var bodyNode = document.CreateElement(Constants.XmlPrefixes.SoapEnv, Constants.XmlRootNames.SoapBody, Constants.XmlNamespaces.SoapEnvelope);
            if (soapBody.Request != null)
            {
                bodyNode.SetAttribute($"xmlns:{Constants.XmlPrefixes.Wsu}", Constants.XmlNamespaces.Wsu);
                bodyNode.SetAttribute($"{Constants.XmlPrefixes.Wsu}:Id", soapBody.Id);
                Serialize(soapBody.Request, document, bodyNode);
            }
            else if (soapBody.Content != null)
            {
                var nodeCopy = document.ImportNode(soapBody.Content, true);
                bodyNode.AppendChild(nodeCopy);
            }

            root.AppendChild(bodyNode);
        }

        #region Serialize SAML information

        private static void Serialize(SamlRequest samlRequest, XmlDocument document, XmlNode root)
        {
            if (samlRequest == null)
            {
                throw new ArgumentNullException(nameof(samlRequest));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }
            
            var samlRequestNode = document.CreateElement(Constants.XmlPrefixes.Samlp, Constants.XmlRootNames.SamlRequest, Constants.XmlNamespaces.Samlp);
            samlRequestNode.SetAttribute($"xmlns:{Constants.XmlPrefixes.Ds}", Constants.XmlNamespaces.Ds);
            samlRequestNode.SetAttribute($"xmlns:{Constants.XmlPrefixes.Saml}", Constants.XmlNamespaces.Saml);
            samlRequestNode.SetAttribute(Constants.XmlAttributeNames.SamlIssueInstant, ConvertToCurrentTime(samlRequest.IssueInstant));
            samlRequestNode.SetAttribute(Constants.XmlAttributeNames.SamlMajorVersion, samlRequest.MajorVersion.ToString());
            samlRequestNode.SetAttribute(Constants.XmlAttributeNames.SamlMinorVersion, samlRequest.MinorVersion.ToString());
            samlRequestNode.SetAttribute(Constants.XmlAttributeNames.SamlRequestId, samlRequest.Id);
            Serialize(samlRequest.SamlAttributeQuery, document, samlRequestNode);
            root.AppendChild(samlRequestNode);
        }

        private static void Serialize(SamlAttributeQuery samlAttributeQuery, XmlDocument document, XmlNode root)
        {
            if (samlAttributeQuery == null)
            {
                throw new ArgumentNullException(nameof(samlAttributeQuery));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var samlAttributeQueryNode = document.CreateElement(Constants.XmlPrefixes.Samlp, Constants.XmlRootNames.SamlAttributeQuery, Constants.XmlNamespaces.Samlp);
            Serialize(samlAttributeQuery.Subject, document, samlAttributeQueryNode);
            if (samlAttributeQuery.Designators != null)
            {
                foreach(var designator in samlAttributeQuery.Designators)
                {
                    Serialize(designator, document, samlAttributeQueryNode);
                }
            }

            root.AppendChild(samlAttributeQueryNode);
        }

        private static void Serialize(SamlSubject samlSubject, XmlDocument document, XmlNode root)
        {
            if (samlSubject == null)
            {
                throw new ArgumentNullException(nameof(samlSubject));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var subjectNode = document.CreateElement(Constants.XmlPrefixes.Saml, Constants.XmlRootNames.SamlSubject, Constants.XmlNamespaces.Saml);
            Serialize(samlSubject.NameIdentifier, document, subjectNode);
            if (samlSubject.SubjectConfirmation != null)
            {
                Serialize(samlSubject.SubjectConfirmation, document, subjectNode);
            }

            root.AppendChild(subjectNode);
        }

        private static void Serialize(SamlNameIdentifier samlNameIdentifier, XmlDocument document, XmlNode root)
        {
            if (samlNameIdentifier == null)
            {
                throw new ArgumentNullException(nameof(samlNameIdentifier));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var nameQualifierNode = document.CreateElement(Constants.XmlPrefixes.Saml, Constants.XmlRootNames.SamlNameIdentifier, Constants.XmlNamespaces.Saml);
            nameQualifierNode.SetAttribute(Constants.XmlAttributeNames.SamlNameIdentifierFormat, samlNameIdentifier.Format);
            nameQualifierNode.SetAttribute(Constants.XmlAttributeNames.SamlNameQualifier, samlNameIdentifier.NameQualifier);
            nameQualifierNode.InnerText = samlNameIdentifier.Value;
            root.AppendChild(nameQualifierNode);
        }

        private static void Serialize(SamlSubjectConfirmation subjectConfirmation, XmlDocument document, XmlNode root)
        {
            if (subjectConfirmation == null)
            {
                throw new ArgumentNullException(nameof(subjectConfirmation));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var subjectConfirmationNode = document.CreateElement(Constants.XmlPrefixes.Saml, Constants.XmlRootNames.SamlSubjectConfirmation, Constants.XmlNamespaces.Saml);
            var confirmationMethodNode = document.CreateElement(Constants.XmlPrefixes.Saml, Constants.XmlRootNames.SamlConfirmationMethod, Constants.XmlNamespaces.Saml);
            var keyInfoNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.KeyInfo, Constants.XmlNamespaces.Ds);
            var x509DataNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.X509Data, Constants.XmlNamespaces.Ds);
            var x509CertificateNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.X509Certificate, Constants.XmlNamespaces.Ds);
            x509CertificateNode.InnerText = Convert.ToBase64String(subjectConfirmation.Certificate.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Cert));
            confirmationMethodNode.InnerText = subjectConfirmation.Method;
            x509DataNode.AppendChild(x509CertificateNode);
            keyInfoNode.AppendChild(x509DataNode);
            subjectConfirmationNode.AppendChild(confirmationMethodNode);
            Serialize(subjectConfirmation.SubjectConfirmationData, document, subjectConfirmationNode);
            subjectConfirmationNode.AppendChild(keyInfoNode);
            root.AppendChild(subjectConfirmationNode);
        }

        private static void Serialize(SamlSubjectConfirmationData subjectConfirmationData, XmlDocument document, XmlNode root)
        {
            if (subjectConfirmationData == null)
            {
                throw new ArgumentNullException(nameof(subjectConfirmationData));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var subjectConfirmationDataNode = document.CreateElement(Constants.XmlPrefixes.Saml, Constants.XmlRootNames.SamlSubjectConfirmationData, Constants.XmlNamespaces.Saml);
            Serialize(subjectConfirmationData.Assertion, document, subjectConfirmationDataNode);
            root.AppendChild(subjectConfirmationDataNode);
        }

        private static void Serialize(SamlAssertion samlAssertion, XmlDocument document, XmlNode root)
        {
            if (samlAssertion == null)
            {
                throw new ArgumentNullException(nameof(samlAssertion));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var assertionNode = document.CreateElement(Constants.XmlPrefixes.Saml, Constants.XmlRootNames.SamlAssertion, Constants.XmlNamespaces.Saml);
            assertionNode.SetAttribute($"xmlns:{Constants.XmlPrefixes.Ns4}", Constants.XmlNamespaces.Ns4);
            assertionNode.SetAttribute(Constants.XmlAttributeNames.SamlAssertionId, samlAssertion.AssertionId);
            assertionNode.SetAttribute(Constants.XmlAttributeNames.SamlIssueInstant, ConvertToCurrentTime(samlAssertion.IssueInstant));
            assertionNode.SetAttribute(Constants.XmlAttributeNames.SamlIssuer, samlAssertion.Issuer);
            assertionNode.SetAttribute(Constants.XmlAttributeNames.SamlMajorVersion, samlAssertion.MajorVersion.ToString());
            assertionNode.SetAttribute(Constants.XmlAttributeNames.SamlMinorVersion, samlAssertion.MinorVersion.ToString());
            Serialize(samlAssertion.Conditions, document, assertionNode);
            Serialize(samlAssertion.AttributeStatement, document, assertionNode);
            root.AppendChild(assertionNode);
        }

        private static void Serialize(SamlConditions conditions, XmlDocument document, XmlNode root)
        {
            if (conditions == null)
            {
                throw new ArgumentNullException(nameof(conditions));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var conditionsNode = document.CreateElement(Constants.XmlPrefixes.Saml, Constants.XmlRootNames.SamlConditions, Constants.XmlNamespaces.Saml);
            conditionsNode.SetAttribute(Constants.XmlAttributeNames.SamlNotBefore, ConvertToCurrentTime(conditions.NotBefore));
            conditionsNode.SetAttribute(Constants.XmlAttributeNames.SamlNotOnOrAfter, ConvertToCurrentTime(conditions.NotOnOrAfter));
            root.AppendChild(conditionsNode);
        }

        private static void Serialize(SamlAttributeStatement attributeStatement, XmlDocument document, XmlNode root)
        {
            if (attributeStatement == null)
            {
                throw new ArgumentNullException(nameof(attributeStatement));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var samlAttributeStatementNode = document.CreateElement(Constants.XmlPrefixes.Saml, Constants.XmlRootNames.SamlAttributeStatement, Constants.XmlNamespaces.Saml);
            Serialize(attributeStatement.Subject, document, samlAttributeStatementNode);
            if (attributeStatement.Attributes != null)
            {
                foreach (var attribute in attributeStatement.Attributes)
                {
                    Serialize(attribute, document, samlAttributeStatementNode);
                }
            }

            root.AppendChild(samlAttributeStatementNode);
        }

        private static void Serialize(SamlAttribute samlAttribute, XmlDocument document, XmlNode root)
        {
            if (samlAttribute == null)
            {
                throw new ArgumentNullException(nameof(samlAttribute));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var samlAttributeNode = document.CreateElement(Constants.XmlPrefixes.Saml, Constants.XmlRootNames.SamlAttribute, Constants.XmlNamespaces.Saml);
            samlAttributeNode.SetAttribute(Constants.XmlAttributeNames.SamlAttributeName, samlAttribute.Name);
            samlAttributeNode.SetAttribute(Constants.XmlAttributeNames.SamlAttributeNamespace, samlAttribute.Namespace);
            var samlAttributeValueNode = document.CreateElement(Constants.XmlPrefixes.Saml, Constants.XmlRootNames.SamlAttributeValue, Constants.XmlNamespaces.Saml);
            samlAttributeValueNode.InnerText = samlAttribute.Value;
            samlAttributeNode.AppendChild(samlAttributeValueNode);
            root.AppendChild(samlAttributeNode);
        }

        private static void Serialize(SamlKeyInfo samlKeyInfo, XmlDocument document, XmlNode root)
        {
            if (samlKeyInfo == null)
            {
                throw new ArgumentNullException(nameof(samlKeyInfo));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var keyInfoNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.KeyInfo, Constants.XmlNamespaces.Ds);
            var x509DataNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.X509Data, Constants.XmlNamespaces.Ds);
            var x509CertificateNode = document.CreateElement(Constants.XmlPrefixes.Ds, Constants.XmlRootNames.X509Certificate, Constants.XmlNamespaces.Ds);
            x509CertificateNode.InnerText = Convert.ToBase64String(samlKeyInfo.Payload.ToArray());
            x509DataNode.AppendChild(x509CertificateNode);
        }

        private static void Serialize(SamlAttributeDesignator samlAttributeDesignator, XmlDocument document, XmlNode root)
        {
            if (samlAttributeDesignator == null)
            {
                throw new ArgumentNullException(nameof(samlAttributeDesignator));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var samlAttributeDesignatorNode = document.CreateElement(Constants.XmlPrefixes.Saml, Constants.XmlRootNames.SamlAttributeDesignator, Constants.XmlNamespaces.Saml);
            samlAttributeDesignatorNode.SetAttribute(Constants.XmlAttributeNames.SamlAttributeName, samlAttributeDesignator.AttributeName);
            samlAttributeDesignatorNode.SetAttribute(Constants.XmlAttributeNames.SamlAttributeNamespace, samlAttributeDesignator.AttributeNamespace);
            root.AppendChild(samlAttributeDesignatorNode);
        }

        #endregion

        #region Common methods

        private static string ConvertToCurrentTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffK");
        }

        private static string ConvertToUtcTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        #endregion
    }
}
