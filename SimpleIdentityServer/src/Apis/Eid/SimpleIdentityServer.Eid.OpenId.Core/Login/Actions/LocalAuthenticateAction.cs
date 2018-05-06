﻿using SimpleIdentityServer.Core.Models;
using SimpleIdentityServer.Eid.OpenId.Core.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace SimpleIdentityServer.Eid.OpenId.Core.Login.Actions
{
    public interface ILocalAuthenticateAction
    {
        ResourceOwner Execute(LocalAuthenticateParameter parameter);
    }

    internal sealed class LocalAuthenticateAction : ILocalAuthenticateAction
    {
        private const string _idName = "urn:be:fgov:person:ssin";

        public ResourceOwner Execute(LocalAuthenticateParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (string.IsNullOrWhiteSpace(parameter.Xml))
            {
                throw new ArgumentNullException(nameof(parameter.Xml));
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(parameter.Xml);
            CheckXmlWellFormed(xmlDocument);
            return ExtractResourceOwner(xmlDocument);
        }

        private static void CheckXmlWellFormed(XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException(nameof(xmlDocument));
            }

            var nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
            nsmgr.AddNamespace(Common.Constants.XmlPrefixes.Ds, Common.Constants.XmlNamespaces.Ds);
            nsmgr.AddNamespace(Common.Constants.XmlPrefixes.Wsse, Common.Constants.XmlNamespaces.Wsse);
            var signatureNode = xmlDocument.SelectSingleNode("//ds:SignatureValue", nsmgr);
            if (signatureNode == null)
            {
                // TODO: Throw an exception.
            }

            var binarySecurityTokenNode = xmlDocument.SelectSingleNode("//wsse:BinarySecurityToken", nsmgr);
            if (binarySecurityTokenNode == null)
            {
                // TODO: Throw an exception.
            }

            var signedInfoNode = xmlDocument.SelectSingleNode("//ds:SignedInfo", nsmgr);
            if (signedInfoNode == null)
            {
                // TODO : Throw an exception.
            }


            var serializer = new XmlDsigExcC14NTransform();
            var doc = new XmlDocument();
            doc.LoadXml(signedInfoNode.OuterXml);
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
            var signature = System.Convert.FromBase64String(signatureNode.InnerText);
            var x509Certificate = new X509Certificate2(Convert.FromBase64String(binarySecurityTokenNode.InnerText));
            var publicKey = x509Certificate.GetRSAPublicKey();
            var isSignatureCorrect = publicKey.VerifyHash(hashResult, signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            if (!isSignatureCorrect)
            {
                // TODO: Throw an exception.
            }

            // TODO: CHECK THE DATE.
        }

        private static void CheckXmlCertificates(XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentException(nameof(xmlDocument));
            }
        }

        private static ResourceOwner ExtractResourceOwner(XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException(nameof(xmlDocument));
            }
            
            var nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
            nsmgr.AddNamespace(Common.Constants.XmlPrefixes.Saml, Common.Constants.XmlNamespaces.Saml);
            nsmgr.AddNamespace(Common.Constants.XmlPrefixes.Ds, Common.Constants.XmlNamespaces.Ds);
            nsmgr.AddNamespace(Common.Constants.XmlPrefixes.Wsse, Common.Constants.XmlNamespaces.Wsse);
            var nodes = xmlDocument.SelectNodes("//saml:Attribute/saml:AttributeValue", nsmgr);
            var claims = new List<Claim>();
            foreach (XmlElement node in nodes)
            {
                var attributes = node.ParentNode.Attributes;
                XmlAttribute attributeName = null;
                foreach (XmlAttribute attribute in attributes)
                {
                    if (attribute.Name == "AttributeName")
                    {
                        attributeName = attribute;
                    }
                }

                if (attributeName == null)
                {
                    continue;
                }

                claims.Add(new Claim(attributeName.Value, node.InnerText));
            }

            var idClaim = claims.First(c => c.Type == _idName);
            return new ResourceOwner
            {
                Id = idClaim.Value,
                Claims = claims
            };
        }
    }
}
