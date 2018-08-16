using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Common.Extensions;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.DocumentManagement.Client.Tests
{
    public class DocumentManagementFactoryFixture
    {
        [Fact]
        public async Task When_Get_Jwks_And_Encrypt_Content()
        {
            const string baseUrl = "http://localhost:60010";
            var docMgClientFactory = new DocumentManagementFactory();
            var jwksKeys = await docMgClientFactory.GetJwksClient().ExecuteAsync(new Uri($"{baseUrl}/jwks"));
            var jwks = jwksKeys.Keys.First();
            var modulus = jwks["n"].ToString().Base64DecodeBytes();
            var exponent = jwks["e"].ToString().Base64DecodeBytes();
            var rsaParameters = new RSAParameters();
            rsaParameters.Modulus = modulus;
            rsaParameters.Exponent = exponent;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParameters);
            var encryptedResult = rsa.Encrypt(Encoding.UTF8.GetBytes("ss"), true);

            string s = "";
        }

        [Fact]
        public async Task WhenCreateDocumentThenOkIsReturned()
        {
            const string baseUrl = "http://localhost:60010";
            const string openIdWellKnownConfiguration = "http://localhost:60000/.well-known/openid-configuration";
            const string umaWellKnownConfiguration = "http://localhost:60004/.well-known/uma2-configuration";
            var identityServerClientFactory = new IdentityServerClientFactory();
            var docMgClientFactory = new DocumentManagementFactory();
            var umaClientFactory = new IdentityServerUmaClientFactory();
            var admGrantedToken = await identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth("SimpleIdServerClient", "SimpleIdServerClient")
                .UsePassword("administrator", "password", "openid", "profile")
                .ResolveAsync(openIdWellKnownConfiguration);
            var umaGrantedToken = await identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth("OfficeAddin", "Cr8cfLttLabTvezn")
                .UseClientCredentials("uma_protection").ResolveAsync(umaWellKnownConfiguration);
            var privateKey = string.Empty;
            using (var rsa = new RSACryptoServiceProvider())
            {
                privateKey = rsa.ToXmlString(true);
            }

            await docMgClientFactory.GetOfficeDocumentClient().Add(new AddOfficeDocumentRequest
            {
                Id = "docid"
            }, baseUrl, admGrantedToken.Content.AccessToken);
            var doc = await docMgClientFactory.GetOfficeDocumentClient().Get("docid", baseUrl, string.Empty);
            /*
            if (doc.ContainsError)
            {
                var permissionResponse = await umaClientFactory.GetPermissionClient().AddByResolution(new PostPermission
                {
                    ResourceSetId = doc.OfficeDocument.UmaResourceId,
                    Scopes = new List<string>()
                    {
                        "read"
                    }
                }, umaWellKnownConfiguration, umaGrantedToken.AccessToken);
                var umaTicketGrantedToken = await identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth("OfficeAddin", "Cr8cfLttLabTvezn")
                    .UseTicketId(permissionResponse.TicketId, admGrantedToken.IdToken)
                    .ResolveAsync(umaWellKnownConfiguration);
                var document = await docMgClientFactory.GetOfficeDocumentClient().Get("docid", baseUrl, umaTicketGrantedToken.AccessToken);
                string gg = "";
                // GET THE UMA RESOURCE ID
                // GET AN ACCESS TOKEN FOR THE UMA RESOURCE ID.
            }
            */
        }
    }
}
