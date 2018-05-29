using SimpleIdentityServer.Common.Saml.Serializers;
using SimpleIdentityServer.Common.Saml.SoapMessages;
using SimpleIdentityServer.Eid.Ehealth.Builders;
using SimpleIdentityServer.Eid.Ehealth.Tlv;
using SimpleIdentityServer.Eid.Sign;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Xunit;

namespace SimpleIdentityServer.Eid.Tests
{
    public class EhealthFixture
    {
        [Fact]
        public void WhenAuthenticateUserWithSamlTokenThenNoExceptionIsThrown()
        {
            string outerXml;
            XmlDocument xmlDocument;
            SoapEnvelope soapEnvelope;
            var beIdCardConnector = new BeIdCardConnector();
            var context = beIdCardConnector.EstablishContext();
            var readers = beIdCardConnector.GetReaders();
            var connection = beIdCardConnector.Connect(readers.First());

            var ehealthSamlTokenRequestBuilder = new EhealthSamlTokenRequestBuilder(); // 1. Construct SAML token.
            var certificate = beIdCardConnector.GetAuthenticateCertificate();
            var tlvParser = new TlvParser();
            var identityPayload = beIdCardConnector.GetIdentity();
            var addressPayload = beIdCardConnector.GetAddress();
            var identity = tlvParser.Parse<Identity>(identityPayload);
            var address = tlvParser.Parse<Address>(addressPayload);
            ehealthSamlTokenRequestBuilder.New(certificate).SetIdentity(identity);

            soapEnvelope = ehealthSamlTokenRequestBuilder.Build();
            var signSamlToken = new SignSamlToken(); // 2. Build signature.
            var signatureNode = signSamlToken.BuildSignatureWithEid(soapEnvelope, "0726", beIdCardConnector); 
            soapEnvelope.Header.Security.Signature = signatureNode;
            var soapSerializer = new SoapMessageSerializer(); // 3. Serialize the request.
            xmlDocument = soapSerializer.Serialize(soapEnvelope);
            outerXml = xmlDocument.OuterXml;
            
            beIdCardConnector.Dispose();

            var nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
            nsmgr.AddNamespace(Common.Saml.Constants.XmlPrefixes.Ds, Common.Saml.Constants.XmlNamespaces.Ds);
            nsmgr.AddNamespace(Common.Saml.Constants.XmlPrefixes.Wsse, Common.Saml.Constants.XmlNamespaces.Wsse);
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
            Assert.True(isSignatureCorrect);
        }
    }
}
