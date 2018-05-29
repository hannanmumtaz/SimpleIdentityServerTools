using SimpleIdentityServer.Common.Saml.Serializers;
using SimpleIdentityServer.Common.Saml.SoapMessages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace SimpleIdentityServer.Eid.Sign
{
    public interface ISignSamlToken
    {
        SoapSignature BuildSignatureWithEid(SoapEnvelope soapEnvelope, string pin, IBeIdCardConnector connector);
    }

    public class SignSamlToken : ISignSamlToken
    {
        public SoapSignature BuildSignatureWithEid(SoapEnvelope soapEnvelope, string pin, IBeIdCardConnector connector)
        {
            if (soapEnvelope == null)
            {
                throw new ArgumentNullException(nameof(soapEnvelope));
            }

            if (string.IsNullOrWhiteSpace(pin))
            {
                throw new ArgumentNullException(nameof(pin));
            }

            if (connector == null)
            {
                throw new ArgumentNullException(nameof(connector));
            }

            var serializer = new SoapMessageSerializer(); // Serialize into XML.
            var xmlDocument = serializer.Serialize(soapEnvelope);
            string xml = null;
            using (var strWriter = new StringWriter())
            {
                using (var xmlTextWriter = XmlWriter.Create(strWriter))
                {
                    xmlDocument.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    xml = strWriter.GetStringBuilder().ToString();
                }
            }
            
            var nsmgr = new XmlNamespaceManager(xmlDocument.NameTable); // 1. Construct the SignedInfo.
            nsmgr.AddNamespace(Common.Saml.Constants.XmlPrefixes.Wsu, Common.Saml.Constants.XmlNamespaces.Wsu);
            nsmgr.AddNamespace(Common.Saml.Constants.XmlPrefixes.SoapEnv, Common.Saml.Constants.XmlNamespaces.SoapEnvelope);
            nsmgr.AddNamespace(Common.Saml.Constants.XmlPrefixes.Wsse, Common.Saml.Constants.XmlNamespaces.Wsse);
            nsmgr.AddNamespace(Common.Saml.Constants.XmlPrefixes.Ds, Common.Saml.Constants.XmlNamespaces.Ds);
            var bodyTokenNode = xmlDocument.SelectSingleNode($"//{Common.Saml.Constants.XmlPrefixes.SoapEnv}:Body", nsmgr);
            var timeStampNode = xmlDocument.SelectSingleNode($"//{Common.Saml.Constants.XmlPrefixes.Wsu}:Timestamp", nsmgr);
            var binaryTokenNode = xmlDocument.SelectSingleNode($"//{Common.Saml.Constants.XmlPrefixes.Wsse}:BinarySecurityToken", nsmgr);
            var timeStampId = timeStampNode.Attributes[$"{Common.Saml.Constants.XmlPrefixes.Wsu}:Id"].Value;
            var binaryTokenId = binaryTokenNode.Attributes[$"{Common.Saml.Constants.XmlPrefixes.Wsu}:Id"].Value;
            var bodyTokenId = bodyTokenNode.Attributes[$"{Common.Saml.Constants.XmlPrefixes.Wsu}:Id"].Value;
            var signatureNode = Canonilize(xml, new[]
            {
                timeStampId,
                binaryTokenId,
                bodyTokenId
            });

            var c14Serializer = new XmlDsigExcC14NTransform();  // 2. Compute the signature value.
            var c14Doc = new XmlDocument();
            c14Doc.LoadXml(signatureNode.FirstChild.OuterXml);
            c14Serializer.LoadInput(c14Doc);
            var c14n = new StreamReader((Stream)c14Serializer.GetOutput(typeof(Stream))).ReadToEnd();
            var signedInfoPayload = Encoding.UTF8.GetBytes(c14n);
            var b64 = Convert.ToBase64String(signedInfoPayload);
            byte[] hashResult = null;
            using (var sha = new SHA1CryptoServiceProvider())
            {
                hashResult = sha.ComputeHash(signedInfoPayload);
            }

            var b64Hash = Convert.ToBase64String(hashResult);
            byte[] signatureValue = null; // 3. Construct the result.
            var certificate = connector.GetAuthenticateCertificate();
            var applicationName = "medikit";
            var digestAlgo = BeIDDigest.Sha1;
            var fileType = FileType.NonRepudiationCertificate;
            var requireSecureReader = false;
            signatureValue = connector.SignWithNoneRepudationCertificate(hashResult, digestAlgo, requireSecureReader, applicationName, pin);

            var signatureValueB64 = Convert.ToBase64String(signatureValue);
            var soapKeyInfo = new SoapKeyInfo(GenerateId("KI"),
                GenerateId("STR"),
                $"#{binaryTokenId}");
            var result = new SoapSignature(GenerateId("SIG"), signatureValueB64, soapKeyInfo);
            var referenceNodes = signatureNode.SelectNodes($"//{Common.Saml.Constants.XmlPrefixes.Ds}:Reference", nsmgr);
            foreach(XmlNode referenceNode in referenceNodes)
            {
                var uri = referenceNode.Attributes["URI"].Value;
                var digestValueNode = referenceNode.SelectSingleNode($"{Common.Saml.Constants.XmlPrefixes.Ds}:DigestValue", nsmgr);
                result.References.Add(new SoapReference(uri, digestValueNode.InnerText));
            }

            return result;
        }

        private static XmlElement Canonilize(string xml, IEnumerable<string> ids)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentNullException(nameof(xml));
            }

            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            var doc = new XmlDsigDocument();
            var env = new XmlDsigExcC14NTransform();
            doc.LoadXml(xml);
            var signedXml = new SignedXmlWithId(doc)
            {
                SigningKey = new RSACryptoServiceProvider()
            };
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
            signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
            foreach (var id in ids)
            {
                var reference = new Reference($"#{id}");
                reference.AddTransform(env);
                reference.DigestMethod = Common.Saml.Constants.XmlNamespaces.Sha1;
                signedXml.AddReference(reference);
            }

            signedXml.ComputeSignature();
            var xmlDigitalSignature = signedXml.GetXml();
            ChangePrefix(xmlDigitalSignature);
            return xmlDigitalSignature;
        }

        private static void ChangePrefix(XmlNode node)
        {
            if (node.ChildNodes == null)
            {
                return;
            }

            node.Prefix = Common.Saml.Constants.XmlPrefixes.Ds;
            foreach (XmlNode child in node.ChildNodes)
            {
                ChangePrefix(child);
            }
        }

        private static string GenerateId(string prefix)
        {
            return $"{prefix}-{Guid.NewGuid().ToString()}".ToLower();
        }
    }
}
