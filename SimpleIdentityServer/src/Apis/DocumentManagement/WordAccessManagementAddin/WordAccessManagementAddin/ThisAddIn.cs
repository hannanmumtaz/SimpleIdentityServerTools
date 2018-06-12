using Microsoft.Office.Interop.Word;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.IO;
using System.Net;
using System.Text;
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

            // ENCRYPT.
            var range = Doc.Range();
            var xml = range.XML;
            var image = ResourceHelper.GetImage("WordAccessManagementAddin.Resources.lock.png");
            var filePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
            var bm = SteganographyHelper.CreateNonIndexedImage(image);
            bm.Save(filePath);
            range.Text = string.Empty;
            var shape = range.InlineShapes.AddPicture(filePath, false, true);
            var encryptedData = EncryptionHelper.Encrypt(officeDocument, xml);
            shape.AlternativeText = Convert.ToBase64String(encryptedData);
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
        }

        private OfficeDocumentResponse GetOfficeDocument(string sidDocumentIdValue, string identityToken)
        {
            var identityServerClientFactory = new IdentityServerClientFactory();
            var identityServerUmaClientFactory = new IdentityServerUmaClientFactory();
            var docMgClientFactory = new DocumentManagementFactory();
            var umaGrantedToken = identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth(Constants.ClientId, Constants.ClientSecret)
                .UseClientCredentials("uma_protection").ResolveAsync(Constants.UmaWellKnownConfiguration)
                .Result;
            var officeDocumentClient = docMgClientFactory.GetOfficeDocumentClient();
            var getDocumentResponse = officeDocumentClient.Get(sidDocumentIdValue, Constants.DocumentApiBaseUrl, string.Empty).Result;
            if (getDocumentResponse.StatusCode != HttpStatusCode.Unauthorized)
            {
                return null;
            }

            var permissionResponse = identityServerUmaClientFactory.GetPermissionClient().AddByResolution(new PostPermission
            {
                ResourceSetId = getDocumentResponse.UmaResourceId,
                Scopes = new[] { "read" }
            }, Constants.UmaWellKnownConfiguration, umaGrantedToken.AccessToken).Result;
            if (permissionResponse == null || string.IsNullOrWhiteSpace(permissionResponse.TicketId))
            {
                return null;
            }

            var grantedToken = identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth(Constants.ClientId, Constants.ClientSecret).UseTicketId(permissionResponse.TicketId, identityToken)
                .ResolveAsync(Constants.UmaWellKnownConfiguration)
                .Result;
            if (grantedToken == null || string.IsNullOrWhiteSpace(grantedToken.AccessToken))
            {
                return null;
            }

            var getOfficeDocumentResponse = officeDocumentClient.Get(sidDocumentIdValue, Constants.DocumentApiBaseUrl, grantedToken.AccessToken).Result;
            if (getOfficeDocumentResponse == null || getOfficeDocumentResponse.ContainsError || getOfficeDocumentResponse.OfficeDocument == null)
            {
                return null;
            }

            return getOfficeDocumentResponse.OfficeDocument;
        }
            
        private void InternalStartup()
        {
            Application.DocumentBeforeClose += HandleDocumentBeforeClose;
            AuthenticationStore.Instance().Authenticated += HandleDecryptDocument;
        }
    }
}
