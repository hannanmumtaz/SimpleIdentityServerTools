using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Core.Common.Models;
using SimpleIdentityServer.Core.Common.Repositories;
using SimpleIdentityServer.Authenticate.Eid.Core.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SimpleIdentityServer.Authenticate.Eid.Core.Login.Actions
{
    public interface ILocalAuthenticateAction
    {
        Task<ResourceOwner> Execute(LocalAuthenticateParameter parameter, string imagePath, string hostUrl);
    }

    internal sealed class LocalAuthenticateAction : ILocalAuthenticateAction
    {
        private const string _idName = "urn:be:fgov:person:ssin";
        private const string _personName = "urn:be:fgov:person:name";
        private const string _pictureName = "urn:be:fgov:person:picture";
        private static readonly Dictionary<string, string> _mappingEidClaimsToOpenIdClaims = new Dictionary<string, string>
        {
            { _idName, SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject },
            { _personName, SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Name },
            { "urn:be:fgov:person:middleName", SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.MiddleName },
            { "urn:be:fgov:person:gender", SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Gender }
        };
        private static readonly Dictionary<string, string> _mappingEidAddressClaimsToOpenIdAddressClaims = new Dictionary<string, string>
        {
            { "urn:be:fgov:address:streetAndNumber", SimpleIdentityServer.Core.Jwt.Constants.StandardAddressClaimNames.StreetAddress },
            { "urn:be:fgov:address:zip", SimpleIdentityServer.Core.Jwt.Constants.StandardAddressClaimNames.PostalCode },
            { "urn:be:fgov:address:municipality", SimpleIdentityServer.Core.Jwt.Constants.StandardAddressClaimNames.Locality }
        };

        private readonly IResourceOwnerRepository _resourceOwnerRepository;

        public LocalAuthenticateAction(IResourceOwnerRepository resourceOwnerRepository)
        {
            _resourceOwnerRepository = resourceOwnerRepository;
        }

        public async Task<ResourceOwner> Execute(LocalAuthenticateParameter parameter, string imagePath, string hostUrl)
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
            var resourceOwner = ExtractResourceOwner(xmlDocument, imagePath, hostUrl);
            var existingResourceowner = await _resourceOwnerRepository.GetAsync(resourceOwner.Id);
            if (existingResourceowner == null)
            {
                await _resourceOwnerRepository.InsertAsync(resourceOwner);
            }

            return resourceOwner;
        }

        private static void CheckXmlWellFormed(XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException(nameof(xmlDocument));
            }

            var nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
            nsmgr.AddNamespace(Common.Saml.Constants.XmlPrefixes.Ds, Common.Saml.Constants.XmlNamespaces.Ds);
            nsmgr.AddNamespace(Common.Saml.Constants.XmlPrefixes.Wsse, Common.Saml.Constants.XmlNamespaces.Wsse);
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

        private static ResourceOwner ExtractResourceOwner(XmlDocument xmlDocument, string imagePath, string hostUrl)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException(nameof(xmlDocument));
            }
            
            var nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
            nsmgr.AddNamespace(Common.Saml.Constants.XmlPrefixes.Saml, Common.Saml.Constants.XmlNamespaces.Saml);
            nsmgr.AddNamespace(Common.Saml.Constants.XmlPrefixes.Ds, Common.Saml.Constants.XmlNamespaces.Ds);
            nsmgr.AddNamespace(Common.Saml.Constants.XmlPrefixes.Wsse, Common.Saml.Constants.XmlNamespaces.Wsse);
            var nodes = xmlDocument.SelectNodes("//saml:Attribute/saml:AttributeValue", nsmgr);
            var claims = new List<Claim>();
            var adr = new JObject();
            var picturePayload = string.Empty;
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

                if(_mappingEidClaimsToOpenIdClaims.ContainsKey(attributeName.Value))
                {
                    claims.Add(new Claim(_mappingEidClaimsToOpenIdClaims[attributeName.Value], node.InnerText));
                }
                
                if (_mappingEidAddressClaimsToOpenIdAddressClaims.ContainsKey(attributeName.Value))
                {
                    adr.Add(_mappingEidAddressClaimsToOpenIdAddressClaims[attributeName.Value], node.InnerText);
                }

                if (attributeName.Value == _personName)
                {
                    claims.Add(new Claim(SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.GivenName, node.InnerText));
                }

                if (attributeName.Value == _pictureName && !string.IsNullOrWhiteSpace(imagePath) && !string.IsNullOrWhiteSpace(node.InnerText))
                {
                    picturePayload = node.InnerText;
                }
            }

            var idClaim = claims.First(c => c.Type == SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject);
            claims.Add(new Claim(SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Address, adr.ToString()));
            if (!string.IsNullOrWhiteSpace(picturePayload))
            {
                var currentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                currentDirectory = Path.Combine(currentDirectory, imagePath);
                if (Directory.Exists(currentDirectory))
                {
                    var picture = StringToByteArray(picturePayload);
                    var picturePath = Path.Combine(currentDirectory, $"{idClaim.Value}.png");
                    File.WriteAllBytes(picturePath, picture);
                    claims.Add(new Claim(SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Picture, $"{hostUrl}/{imagePath}/{idClaim.Value}.png"));
                }
            }

            return new ResourceOwner
            {
                Id = idClaim.Value,
                IsLocalAccount = false,
                TwoFactorAuthentication = TwoFactorAuthentications.NONE,
                Claims = claims
            };
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
