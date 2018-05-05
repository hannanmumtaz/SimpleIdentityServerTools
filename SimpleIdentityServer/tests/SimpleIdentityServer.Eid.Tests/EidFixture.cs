using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleIdentityServer.Eid.Common.Serializers;
using SimpleIdentityServer.Eid.Common.SoapMessages;
using SimpleIdentityServer.Eid.Ehealth.Builders;
using SimpleIdentityServer.Eid.Sign;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace SimpleIdentityServer.Eid.Tests
{
    [TestClass]
    public class EidFixture
    {
        [TestMethod]
        public void WhenCheckCertificates()
        {
            // TODO : 
            // EVENT. When the SAML token is received
            // 1. Check the signature
            // 2. Get the certificate
            // 3. Retrieve the certificate from the feed : http://certs.pki.belgium.be/
            // 4. Check the certificate doesn't belong to the clr.
            var cert = "MIID7zCCAtegAwIBAgIQEAAAAAAAjzo9FBi9BAvSgTANBgkqhkiG9w0BAQUFADA1MQswCQYDVQQGEwJCRTEVMBMGA1UEAxMMRm9yZWlnbmVyIENBMQ8wDQYDVQQFEwYyMDE0MDIwHhcNMTQwMjA1MTIzMzEyWhcNMTkwMTMwMjM1OTU5WjB3MQswCQYDVQQGEwJGUjEoMCYGA1UEAxMfVGhpZXJyeSBIYWJhcnQgKEF1dGhlbnRpY2F0aW9uKTEPMA0GA1UEBBMGSGFiYXJ0MRcwFQYDVQQqEw5UaGllcnJ5IFJvYmVydDEUMBIGA1UEBRMLODkxMDA3Mzk1NzMwgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAI4At/6KxOpRKiFEluuMVRaP+YI4gKKeVpl10CY+vPln9P3dvUh7gdNWfEN2Fn0LzvOslVDWZeb+A8/HBhPePpTJ9sACPJG+BjcLKZYKwjc6eEKAHmDTqClTuvrSi5hzpEuDJ7gHQmzJcochdcKRr06MIg3wP7P8s91VxSJoKJ0/AgMBAAGjggE7MIIBNzAfBgNVHSMEGDAWgBTD/rhgVgSUYRHFYhTWJlYgS27SvTBwBggrBgEFBQcBAQRkMGIwNgYIKwYBBQUHMAKGKmh0dHA6Ly9jZXJ0cy5laWQuYmVsZ2l1bS5iZS9iZWxnaXVtcnMyLmNydDAoBggrBgEFBQcwAYYcaHR0cDovL29jc3AuZWlkLmJlbGdpdW0uYmUvMjBEBgNVHSAEPTA7MDkGB2A4CQEBBwIwLjAsBggrBgEFBQcCARYgaHR0cDovL3JlcG9zaXRvcnkuZWlkLmJlbGdpdW0uYmUwOQYDVR0fBDIwMDAuoCygKoYoaHR0cDovL2NybC5laWQuYmVsZ2l1bS5iZS9laWRmMjAxNDAyLmNybDAOBgNVHQ8BAf8EBAMCB4AwEQYJYIZIAYb4QgEBBAQDAgWgMA0GCSqGSIb3DQEBBQUAA4IBAQBSz7layb5HgSQM+ZB2np1/gAUJ+8wuxoU62vxGrj7T1K5tmRK5WeX4rpEJ8h0Nv1169GGng77xlAdE9s8noQXQ063SuBg/QAXSYTRF2T+g9rZinC12aljo/bLz8jWSBY8AM4GZQcVAQ31aD+mWzO0DPwVtDvhl0/6jFVprKFDhAnPJvzMQe7a6zwi5lL+GXIKrnNhlvfQDtxJRV4Jr1vps7ZBjXnAOuatMIxlJ9eafU3dgQ2TvTpwSVDV8UgrInhKD9cLBzymli4LeMWyD6qC3SwfDwRVODqNSIogBnSH91fn3nZUDKpUYfOCSGxNozGYZsVnqwO0jJf4E69O5lUgg";
            var chain = new X509Chain();
            var belgiumRs2 = new X509Certificate2("belgiumrs2.crt");
            var foreigner = new X509Certificate2("foreigner201402.crt"); // Check revocation list.
            var authCert = new X509Certificate2(System.Convert.FromBase64String(cert));            
            // var authCert = new X509Certificate2("SimpleIdServer.pfx");
            var isValid = chain.Build(authCert);
            var isSelfSigned = authCert.SubjectName.RawData.SequenceEqual(authCert.IssuerName.RawData);
            var isOk = foreigner.SubjectName.RawData.SequenceEqual(authCert.IssuerName.RawData);
            var c = authCert.Verify();
            string s = "";
        }

        [TestMethod]
        public void WhenGenerateEidSamlRequest()
        {
            string outerXml;
            XmlDocument xmlDocument;
            SoapEnvelope soapEnvelope;
            var beIdCardConnector = new BeIdCardConnector();
            var context = beIdCardConnector.EstablishContext();
            var readers = beIdCardConnector.GetReaders();
            var connect = beIdCardConnector.Connect(readers.First());

            // 1. Get the identity.
            var identity = beIdCardConnector.GetIdentity();
            // 2. Get the address.
            var addr = beIdCardConnector.GetAddress();
            // 3. Get the auth certificate.
            var authCertificate = beIdCardConnector.GetAuthenticateCertificate();

            context.Release();
            context.Dispose();
            string s = "";

            /*

            var certificate = beIdCardConnector.GetAuthenticateCertificate();
            var ehealthSamlTokenRequestBuilder = new EhealthSamlTokenRequestBuilder(); // 1. Construct SAML token.
            soapEnvelope = ehealthSamlTokenRequestBuilder.New(certificate).Build();
            var signSamlToken = new SignSamlToken();
            var signatureNode = signSamlToken.BuildSignatureWithEid(soapEnvelope, "0726", beIdCardConnector); // 2. Build signature.
            soapEnvelope.Header.Security.Signature = signatureNode;
            var soapSerializer = new SoapMessageSerializer(); // 3. Serialize the request.
            xmlDocument = soapSerializer.Serialize(soapEnvelope);
            outerXml = xmlDocument.OuterXml;
            context.Release();
            context.Dispose();

            var schemaSet = new XmlSchemaSet(); // 4. Check XML against XSD.
            schemaSet.Add("urn:oasis:names:tc:SAML:1.0:protocol", "oasis-sstc-saml-schema-protocol-1.1.xsd");
            schemaSet.Add("urn:oasis:names:tc:SAML:1.0:assertion", "oasis-sstc-saml-schema-assertion-1.1.xsd");
            schemaSet.Add("http://www.w3.org/2000/09/xmldsig#", "xmldsig-core-schema.xsd");
            var compiledSchemas = schemaSet.Schemas();
            var settings = new XmlReaderSettings();
            int nbErrors = 0;
            settings.ValidationEventHandler += new ValidationEventHandler((o, e) =>
            {
                nbErrors++;
            });
            settings.ValidationType = ValidationType.Schema;
            using (var strReader = new StringReader(outerXml))
            {
                var vreader = XmlReader.Create(strReader, settings);
                while (vreader.Read()) { }
            }

            Assert.IsTrue(nbErrors == 0);
            var nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
            nsmgr.AddNamespace(Common.Constants.XmlPrefixes.Ds, Common.Constants.XmlNamespaces.Ds);
            nsmgr.AddNamespace(Common.Constants.XmlPrefixes.Wsse, Common.Constants.XmlNamespaces.Wsse);
            var signatureValue = xmlDocument.SelectSingleNode("//ds:SignatureValue", nsmgr).InnerText; // 5. Check signature value.
            var binarySecurityToken = xmlDocument.SelectSingleNode("//wsse:BinarySecurityToken", nsmgr).InnerText;
            var signedInfo = xmlDocument.SelectSingleNode("//ds:SignedInfo", nsmgr).OuterXml;
            var serializer = new XmlDsigExcC14NTransform();
            var doc = new XmlDocument();
            doc.LoadXml(signedInfo);
            serializer.LoadInput(doc);
            var c14n = new StreamReader((Stream)serializer.GetOutput(typeof(Stream))).ReadToEnd();
            var signedInfoPayload = Encoding.UTF8.GetBytes(c14n);
            var b64 = Convert.ToBase64String(signedInfoPayload);
            byte[] hashResult = null;
            using (var sha = new SHA1CryptoServiceProvider())
            {
                hashResult = sha.ComputeHash(signedInfoPayload);
            }

            var b64Hash = Convert.ToBase64String(hashResult);
            var signature = System.Convert.FromBase64String(signatureValue);
            var x509Certificate = new X509Certificate2(Convert.FromBase64String(binarySecurityToken));
            var publicKey = x509Certificate.GetRSAPublicKey();
            var isSignatureCorrect = publicKey.VerifyHash(hashResult, signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            Assert.IsTrue(isSignatureCorrect);
            */
        }
    }
}
