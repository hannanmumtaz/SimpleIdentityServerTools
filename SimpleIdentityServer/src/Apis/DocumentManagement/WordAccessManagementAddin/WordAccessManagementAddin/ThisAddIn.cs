using Microsoft.Office.Interop.Word;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.IO;
using System.Net;
using WordAccessManagementAddin.Extensions;
using WordAccessManagementAddin.Helpers;
using WordAccessManagementAddin.Stores;

namespace WordAccessManagementAddin
{
    public partial class ThisAddIn
    {
        private void HandleDocumentBeforeClose(Document Doc, ref bool Cancel)
        {
            var authenticateStore = AuthenticationStore.Instance();
            if (string.IsNullOrWhiteSpace(authenticateStore.IdentityToken))
            {
                return;
            }

            string sidDocumentIdValue;
            if (!Doc.TryGetVariable(Constants.VariableName, out sidDocumentIdValue))
            {
                return;
            }

            var officeDocument = GetOfficeDocument(sidDocumentIdValue, authenticateStore.IdentityToken);
            if (officeDocument == null)
            {
                return;
            }

            // Encrypt the content.
            var encryptionHelper = new EncryptionHelper();
            var encryptedData = encryptionHelper.Encrypt(Doc).Result;

            // Insert the image.
            var range = Doc.Range();
            var image = ResourceHelper.GetImage("WordAccessManagementAddin.Resources.lock.png");
            var filePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
            var bm = SteganographyHelper.CreateNonIndexedImage(image);
            bm.Save(filePath);
            range.Text = string.Empty;
            var shape = range.InlineShapes.AddPicture(filePath, false, true);
            shape.AlternativeText = encryptedData;
            File.Delete(filePath);
            Doc.Save();
        }

        private void HandleDecryptDocument(object sender, EventArgs e)
        {
            var authenticateStore = AuthenticationStore.Instance();
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;
            string sidDocumentIdValue;
            if (!activeDocument.TryGetVariable(Constants.VariableName, out sidDocumentIdValue))
            {
                return;
            }

            var range = activeDocument.Range();
            var shapes = range.InlineShapes;
            foreach (InlineShape shape in shapes)
            {
                if (shape.Type != WdInlineShapeType.wdInlineShapePicture || string.IsNullOrWhiteSpace(shape.AlternativeText))
                {
                    continue;
                }

                // DECRYPT
                var b64Encoded = shape.AlternativeText;
                var xml = DecryptOfficeDocument(sidDocumentIdValue, authenticateStore.IdentityToken, b64Encoded);
                shape.Range.InsertXML(xml);
                // var decodedPayload = Convert.FromBase64String(b64Encoded);
                // var decryptedData = EncryptionHelper.Decrypt(officeDocument, decodedPayload);
                // var xml = Encoding.UTF8.GetString(decryptedData);
                // shape.Range.InsertXML(xml);
            }

            // 1. If the file has not been protected then do nothing
            // 2. Pass the encrypted AES key to the service
            // 3. If the user is authorized then a decrypted version of AES key is returned & used to decrypt the document
            /*
            var authenticateStore = AuthenticationStore.Instance();
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;
            string sidDocumentIdValue;
            if (!activeDocument.TryGetVariable(Constants.VariableName, out sidDocumentIdValue))
            {
                return;
            }

            var officeDocument = GetOfficeDocument(sidDocumentIdValue, authenticateStore.IdentityToken);
            if (officeDocument == null)
            {
                return;
            }
            
            var range = activeDocument.Range();
            var shapes = range.InlineShapes;
            foreach (InlineShape shape in shapes)
            {
                if (shape.Type != WdInlineShapeType.wdInlineShapePicture || string.IsNullOrWhiteSpace(shape.AlternativeText))
                {
                    continue;
                }

                // DECRYPT
                var b64Encoded = shape.AlternativeText;
                var decodedPayload = Convert.FromBase64String(b64Encoded);
                var decryptedData = EncryptionHelper.Decrypt(officeDocument, decodedPayload);
                var xml = Encoding.UTF8.GetString(decryptedData);
                shape.Range.InsertXML(xml);
            }
            */
        }

        private OfficeDocumentResponse GetOfficeDocument(string sidDocumentIdValue, string identityToken)
        {
            var identityServerClientFactory = new IdentityServerClientFactory();
            var identityServerUmaClientFactory = new IdentityServerUmaClientFactory();
            var docMgClientFactory = new DocumentManagementFactory();
            var umaGrantedToken = identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth(Constants.ClientId, Constants.ClientSecret)
                .UseClientCredentials("uma_protection").ResolveAsync(Constants.UmaWellKnownConfiguration)
                .Result;
            if (umaGrantedToken.ContainsError)
            {
                return null;
            }

            var officeDocumentClient = docMgClientFactory.GetOfficeDocumentClient();
            // 1. Try to get the document without access token.
            var getDocumentResponse = officeDocumentClient.GetResolve(sidDocumentIdValue, Constants.DocumentApiConfiguration, string.Empty).Result;
            if (getDocumentResponse.HttpStatus != HttpStatusCode.Unauthorized)
            {
                return null;
            }

            // 2. Get a ticket id (scope = read) for the UMA resource.
            var permissionResponse = identityServerUmaClientFactory.GetPermissionClient().AddByResolution(new PostPermission
            {
                ResourceSetId = getDocumentResponse.UmaResourceId,
                Scopes = new[] { "read" }
            }, Constants.UmaWellKnownConfiguration, umaGrantedToken.Content.AccessToken).Result;
            if (permissionResponse.ContainsError)
            {
                return null;
            }

            // 3. Get an access token via the uma grant_type.
            var grantedToken = identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth(Constants.ClientId, Constants.ClientSecret).UseTicketId(permissionResponse.Content.TicketId, identityToken)
                .ResolveAsync(Constants.UmaWellKnownConfiguration)
                .Result;
            if (grantedToken.ContainsError)
            {
                return null;
            }

            // 4. Get the document.
            var getOfficeDocumentResponse = officeDocumentClient.GetResolve(sidDocumentIdValue, Constants.DocumentApiConfiguration, grantedToken.Content.AccessToken).Result;
            if (getOfficeDocumentResponse.ContainsError)
            {
                return null;
            }

            return getOfficeDocumentResponse.OfficeDocument;
        }

        private string DecryptOfficeDocument(string sidDocumentIdValue, string identityToken, string content)
        {
            var identityServerClientFactory = new IdentityServerClientFactory();
            var identityServerUmaClientFactory = new IdentityServerUmaClientFactory();
            var docMgClientFactory = new DocumentManagementFactory();
            /*
            // GET UMA TOKEN & PASS TO THE DECRYPT OPERATION.
            var umaGrantedToken = identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth(Constants.ClientId, Constants.ClientSecret)
                .UseClientCredentials("uma_protection").ResolveAsync(Constants.UmaWellKnownConfiguration)
                .Result;
            if (umaGrantedToken.ContainsError)
            {
                return null;
            }
            */

            var splittedContent = content.Split('.');
            var kid = splittedContent[0];
            var credentials = splittedContent[1];
            var encryptedContent = splittedContent[2];
            var decryptedResult = docMgClientFactory.GetOfficeDocumentClient().DecryptResolve(new DecryptDocumentRequest
            {
                DocumentId = sidDocumentIdValue,
                Credentials = credentials,
                Kid = kid
            }, Constants.DocumentApiConfiguration, "token").Result;
            var encryptionHelper = new EncryptionHelper();
            return encryptionHelper.Decrypt(encryptedContent, decryptedResult.Content);
        }

        private void InternalStartup()
        {            
            Application.DocumentBeforeClose += HandleDocumentBeforeClose;
            AuthenticationStore.Instance().Authenticated += HandleDecryptDocument;
        }
    }
}
