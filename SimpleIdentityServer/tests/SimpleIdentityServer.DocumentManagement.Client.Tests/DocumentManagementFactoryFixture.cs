using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.DocumentManagement.Client.Tests
{
    public class DocumentManagementFactoryFixture
    {
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
                Id = "docid",
                EncAlg = Common.DTOs.Responses.OfficeDocumentEncAlgorithms.RSA,
                EncPassword = "pass",
                EncSalt = "salt"
            }, baseUrl, admGrantedToken.AccessToken);
            var doc = await docMgClientFactory.GetOfficeDocumentClient().Get("docid", baseUrl, string.Empty);
            if (doc.ContainsError)
            {
                var permissionResponse = await umaClientFactory.GetPermissionClient().AddByResolution(new PostPermission
                {
                    ResourceSetId = doc.UmaResourceId,
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
        }
    }
}
