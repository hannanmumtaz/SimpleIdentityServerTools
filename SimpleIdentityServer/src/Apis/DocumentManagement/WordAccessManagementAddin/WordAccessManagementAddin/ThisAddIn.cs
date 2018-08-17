using Microsoft.Office.Interop.Word;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System;
using System.IO;
using WordAccessManagementAddin.Extensions;
using WordAccessManagementAddin.Helpers;
using WordAccessManagementAddin.Stores;

namespace WordAccessManagementAddin
{
    public partial class ThisAddIn
    {
        /// <summary>
        /// Encrypt the document.
        /// </summary>
        /// <param name="Doc"></param>
        /// <param name="Cancel"></param>
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

        /// <summary>
        /// Decrypt the document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                if (string.IsNullOrWhiteSpace(xml))
                {
                    return;
                }

                shape.Range.InsertXML(xml);
            }
        }

        private OfficeDocumentResponse GetOfficeDocument(string sidDocumentIdValue, string identityToken)
        {
            var documentManagementFactory = new DocumentManagementFactory();
            var officeDocumentStore = OfficeDocumentStore.Instance();
            var umaResourceId = officeDocumentStore.GetUmaResourceId(sidDocumentIdValue).Result;
            if (string.IsNullOrWhiteSpace(umaResourceId))
            {
                return null;
            }

            var grantedToken = officeDocumentStore.GetOfficeDocumentAccessTokenViaUmaGrantType(umaResourceId).Result;
            if (grantedToken == null)
            {
                return null;
            }

            var getOfficeDocumentResponse = documentManagementFactory.GetOfficeDocumentClient().GetResolve(sidDocumentIdValue, Constants.DocumentApiConfiguration, grantedToken.AccessToken).Result;
            if (getOfficeDocumentResponse.ContainsError)
            {
                return null;
            }

            return getOfficeDocumentResponse.OfficeDocument;
        }

        private string DecryptOfficeDocument(string sidDocumentIdValue, string identityToken, string content)
        {
            var splittedContent = content.Split('.');
            if (splittedContent.Length != 3)
            {
                return null;
            }

            var identityServerClientFactory = new IdentityServerClientFactory();
            var identityServerUmaClientFactory = new IdentityServerUmaClientFactory();
            var documentManagementFactory = new DocumentManagementFactory();
            var officeDocumentStore = OfficeDocumentStore.Instance();
            var encryptionHelper = new EncryptionHelper();
            var umaResourceId = officeDocumentStore.GetUmaResourceId(sidDocumentIdValue).Result;
            if (string.IsNullOrWhiteSpace(umaResourceId))
            {
                return null;
            }

            var grantedToken = officeDocumentStore.GetOfficeDocumentAccessTokenViaUmaGrantType(umaResourceId).Result;
            if (grantedToken == null)
            {
                return null;
            }

            var kid = splittedContent[0];
            var credentials = splittedContent[1];
            var encryptedContent = splittedContent[2];
            var decryptedResult = documentManagementFactory.GetOfficeDocumentClient().DecryptResolve(new DecryptDocumentRequest
            {
                DocumentId = sidDocumentIdValue,
                Credentials = credentials,
                Kid = kid
            }, Constants.DocumentApiConfiguration, grantedToken.AccessToken).Result;
            return encryptionHelper.Decrypt(encryptedContent, decryptedResult.Content);
        }

        private void InternalStartup()
        {            
            Application.DocumentBeforeClose += HandleDocumentBeforeClose;
            OfficeDocumentStore.Instance().Decrypted += HandleDecryptDocument;
            AuthenticationStore.Instance().Restore();
        }
    }
}
