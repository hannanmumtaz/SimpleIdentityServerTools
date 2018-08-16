using SimpleIdentityServer.Core.Common.Extensions;
using SimpleIdentityServer.DocumentManagement.EF;
using SimpleIdentityServer.DocumentManagement.EF.Models;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace SimpleIdentityServer.DocumentManagement.Startup.Extensions
{
    internal static class DocumentManagementDbContextExtensions
    {
        public static void EnsureSeedData(this DocumentManagementDbContext context)
        {
            InsertJsonWebKeys(context);
            context.SaveChanges();
        }
        
        private static void InsertJsonWebKeys(DocumentManagementDbContext context)
        {
            if (!context.JsonWebKeys.Any())
            {
                var serializedRsa = string.Empty;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    using (var provider = new RSACryptoServiceProvider())
                    {
                        serializedRsa = provider.ToXmlStringNetCore(true);
                    }
                }
                else
                {
                    using (var rsa = new RSAOpenSsl())
                    {
                        serializedRsa = rsa.ToXmlStringNetCore(true);
                    }
                }

                context.JsonWebKeys.AddRange(new[]
                {
                    new OfficeDocumentJsonWebKey
                    {
                        Kid = "1",
                        Kty = KeyType.RSA,
                        SerializedKey = serializedRsa,
                    }
                });
            }
        }
    }
}
