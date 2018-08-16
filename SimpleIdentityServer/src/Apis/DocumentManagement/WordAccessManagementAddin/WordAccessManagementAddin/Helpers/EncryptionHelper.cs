using Microsoft.Office.Interop.Word;
using SimpleIdentityServer.Core.Common.Extensions;
using SimpleIdentityServer.DocumentManagement.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WordAccessManagementAddin.Helpers
{
    internal class EncryptionHelper
    {
        public async Task<string> Encrypt(Document document)
        {
            var range = document.Range();
            var xml = range.XML;
            var salt = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var saltBytes = Encoding.UTF8.GetBytes(salt);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var xmlPayload = Encoding.UTF8.GetBytes(xml);
            // Encrypt document with sym key.
            var encryptedPayloadBase64 = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(xmlPayload, 0, xmlPayload.Length);
                        cs.Close();
                    }

                    var encryptedPayload = ms.ToArray();
                    encryptedPayloadBase64 = Convert.ToBase64String(encryptedPayload);
                }
            }

            // Encrypt the credentials with asym key.
            var docMgClientFactory = new DocumentManagementFactory();
            var jwksKeys = await docMgClientFactory.GetJwksClient().ExecuteAsync(new Uri($"{Constants.DocumentApiBaseUrl}/jwks")).ConfigureAwait(false);
            var jwks = jwksKeys.Keys.First();
            var modulus = jwks["n"].ToString().Base64DecodeBytes();
            var exponent = jwks["e"].ToString().Base64DecodeBytes();
            var kid = jwks["kid"].ToString();
            var rsaParameters = new RSAParameters();
            rsaParameters.Modulus = modulus;
            rsaParameters.Exponent = exponent;
            var credentials = Encoding.UTF8.GetBytes($"{password}.{salt}");
            var encryptedBase64 = string.Empty;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParameters);
                var encrypted = rsa.Encrypt(credentials, true);
                encryptedBase64 = Convert.ToBase64String(encrypted);
            }

            // Returns concatenated result.
            return $"{kid}.{encryptedBase64}.{encryptedPayloadBase64}";
        }

        public string Decrypt(string toBeDecrypted, DecryptedResponse decryptedResponse)
        {
            byte[] decryptedBytes = null;
            var bytesToBeDecrypted = Convert.FromBase64String(toBeDecrypted);
            var saltBytes = Encoding.UTF8.GetBytes(decryptedResponse.Salt);
            var passwordBytes = Encoding.UTF8.GetBytes(decryptedResponse.Password);
            using (MemoryStream ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
