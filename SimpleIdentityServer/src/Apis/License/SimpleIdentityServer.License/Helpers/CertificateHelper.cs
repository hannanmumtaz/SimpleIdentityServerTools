using Newtonsoft.Json;
using SimpleIdentityServer.License.Exceptions;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SimpleIdentityServer.License.Helpers
{
    internal static class CertificateHelper
    {
        public static string Sign(X509Certificate2 certificate, LicenseFile licenseFile)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            if (licenseFile == null)
            {
                throw new ArgumentNullException(nameof(licenseFile));
            }

            if (!certificate.HasPrivateKey || certificate.PrivateKey == null || !(certificate.PrivateKey is RSACryptoServiceProvider))
            {
                throw new NoPrivateKeyException();
            }

            var jsonObj = JsonConvert.SerializeObject(licenseFile);
            var jsonb64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonObj));
            var privateKey = (RSACryptoServiceProvider)certificate.PrivateKey;
            byte[] hashPayload;
            using (var shaMg = SHA1Managed.Create())
            {
                hashPayload = shaMg.ComputeHash(Encoding.UTF8.GetBytes(jsonb64));
            }

            var signature = privateKey.SignHash(hashPayload, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            var b64Signature = Convert.ToBase64String(signature);
            return $"{jsonb64}.{b64Signature}";
        }

        public static bool CheckSignature(X509Certificate2 certificate, string jsonb64, string signature)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }
            if (string.IsNullOrWhiteSpace(jsonb64))
            {
                throw new ArgumentNullException(nameof(jsonb64));
            }

            if (string.IsNullOrWhiteSpace(signature))
            {
                throw new ArgumentNullException(nameof(signature));
            }

            if (certificate.PublicKey == null || !(certificate.PublicKey.Key is RSACryptoServiceProvider))
            {
                throw new NoPublicKeyException();
            }

            byte[] hashPayload;
            using (var shaMg = SHA1Managed.Create())
            {
                hashPayload = shaMg.ComputeHash(Encoding.UTF8.GetBytes(jsonb64));
            }

            var publicKey = (RSACryptoServiceProvider)certificate.PublicKey.Key;
            return publicKey.VerifyHash(hashPayload, Convert.FromBase64String(signature), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }
    }
}
