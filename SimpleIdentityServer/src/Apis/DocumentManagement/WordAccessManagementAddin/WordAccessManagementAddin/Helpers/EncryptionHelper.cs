using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WordAccessManagementAddin.Helpers
{
    internal static class EncryptionHelper
    {
        public static byte[] Encrypt(OfficeDocumentResponse office, string toEncrypt)
        {
            byte[] encryptedBytes = null;
            var saltBytes = Encoding.UTF8.GetBytes(office.EncSalt);
            var passwordBytes = Encoding.UTF8.GetBytes(office.EncPassword);
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(toEncrypt);
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
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static byte[] Decrypt(OfficeDocumentResponse office, string toDecrypt)
        {
            return Decrypt(office, Encoding.UTF8.GetBytes(toDecrypt));
        }

        public static byte[] Decrypt(OfficeDocumentResponse office, byte[] bytesToBeDecrypted)
        {
            byte[] decryptedBytes = null;
            var saltBytes = Encoding.UTF8.GetBytes(office.EncSalt);
            var passwordBytes = Encoding.UTF8.GetBytes(office.EncPassword);
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

            return decryptedBytes;
        }
    }
}
