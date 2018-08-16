using SimpleIdentityServer.Core.Common;
using SimpleIdentityServer.Core.Common.Extensions;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace SimpleIdentityServer.DocumentManagement.Core.Jwks
{
    public interface IJsonWebKeyEnricher
    {
        Dictionary<string, object> GetPublicKeyInformation(OfficeDocumentJsonWebKeyResponse jsonWebKey);
        Dictionary<string, object> GetJsonWebKeyInformation(OfficeDocumentJsonWebKeyResponse jsonWebKey);
    }

    internal sealed class JsonWebKeyEnricher : IJsonWebKeyEnricher
    {
        private readonly Dictionary<KeyTypeResponse, Action<Dictionary<string, object>, OfficeDocumentJsonWebKeyResponse>> _mappingKeyTypeAndPublicKeyEnricher;

        public JsonWebKeyEnricher()
        {
            _mappingKeyTypeAndPublicKeyEnricher = new Dictionary<KeyTypeResponse, Action<Dictionary<string, object>, OfficeDocumentJsonWebKeyResponse>>
            {
                {
                    KeyTypeResponse.RSA, SetRsaPublicKeyInformation
                }
            };
        }

        public Dictionary<string, object> GetPublicKeyInformation(OfficeDocumentJsonWebKeyResponse jsonWebKey)
        {
            var result = new Dictionary<string, object>();
            var enricher = _mappingKeyTypeAndPublicKeyEnricher[jsonWebKey.Kty];
            enricher(result, jsonWebKey);
            return result;
        }

        public Dictionary<string, object> GetJsonWebKeyInformation(OfficeDocumentJsonWebKeyResponse jsonWebKey)
        {
            return new Dictionary<string, object>
            {
                {
                    "kty", "RSA"
                },
                {
                    "use", "enc"
                },
                {
                    "alg", "RS256"
                },
                {
                    "kid", jsonWebKey.Kid
                }
            };
        }

        public void SetRsaPublicKeyInformation(Dictionary<string, object> result, OfficeDocumentJsonWebKeyResponse jsonWebKey)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (var provider = new RSACryptoServiceProvider())
                {
                    provider.FromXmlStringNetCore(jsonWebKey.SerializedKey);
                    var rsaParameters = provider.ExportParameters(false);
                    // Export the modulus
                    var modulus = rsaParameters.Modulus.Base64EncodeBytes();
                    // Export the exponent
                    var exponent = rsaParameters.Exponent.Base64EncodeBytes();

                    result.Add("n", modulus);
                    result.Add("e", exponent);
                }
            }
            else
            {
                using (var provider = new RSAOpenSsl())
                {
                    provider.FromXmlStringNetCore(jsonWebKey.SerializedKey);
                    var rsaParameters = provider.ExportParameters(false);
                    // Export the modulus
                    var modulus = rsaParameters.Modulus.Base64EncodeBytes();
                    // Export the exponent
                    var exponent = rsaParameters.Exponent.Base64EncodeBytes();

                    result.Add("n", modulus);
                    result.Add("e", exponent);
                }
            }
        }
    }
}
