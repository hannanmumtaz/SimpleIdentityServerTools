using Microsoft.Office.Interop.Word;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System;
using System.IO;
using System.Runtime.InteropServices;
using WordAccessManagementAddin.Extensions;
using WordAccessManagementAddin.Helpers;
using WordAccessManagementAddin.Stores;
using System.Linq;

namespace WordAccessManagementAddin
{
    public partial class ThisAddIn
    {
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

        /// <summary>
        /// Encrypt the document.
        /// </summary>
        /// <param name="Doc"></param>
        /// <param name="Cancel"></param>
        private void HandleDocumentBeforeClose(Document Doc, ref bool Cancel)
        {
            string sidDocumentIdValue;
            if (!Doc.TryGetVariable(Constants.VariableName, out sidDocumentIdValue))
            {
                return;
            }

            string isEncryptedStr;
            bool isEncrypted = false;
            if(Doc.TryGetVariable(Constants.IsEncryptedVariableName, out isEncryptedStr))
            {
                bool.TryParse(isEncryptedStr, out isEncrypted);
            }

            if (isEncrypted)
            {
                return;
            }

            // Encrypt the content.
            var encryptionHelper = new EncryptionHelper();
            var encryptedResult = encryptionHelper.Encrypt(Doc, sidDocumentIdValue).Result;
            if (!string.IsNullOrWhiteSpace(AuthenticationStore.Instance().IdentityToken))
            {
                var officeDocumentStore = OfficeDocumentStore.Instance();
                officeDocumentStore.StoreDecryption(sidDocumentIdValue, new DecryptedResponse
                {
                    Password = encryptedResult.Password,
                    Salt = encryptedResult.Salt
                });
            }

            // Insert the image.
            var range = Doc.Range();
            var image = ResourceHelper.GetImage("WordAccessManagementAddin.Resources.lock.png");
            var filePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
            var bm = SteganographyHelper.CreateNonIndexedImage(image);
            bm.Save(filePath);
            range.Text = string.Empty;
            var shape = range.InlineShapes.AddPicture(filePath, false, true);
            shape.AlternativeText = encryptedResult.Content;
            File.Delete(filePath);
            SetEncrypted(Doc, "true");
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
                SetEncrypted(activeDocument, "false");
                activeDocument.Save();
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

            var encryptionHelper = new EncryptionHelper();
            var kid = splittedContent[0];
            var credentials = splittedContent[1];
            var encryptedContent = splittedContent[2];
            var officeDocumentStore = OfficeDocumentStore.Instance();
            var decryptionResponse = officeDocumentStore.RestoreDecryption(sidDocumentIdValue);
            if (decryptionResponse != null)
            {
                try
                {
                    var result = encryptionHelper.Decrypt(encryptedContent, decryptionResponse);
                    return result;
                }
                catch(Exception) { }
            }


            var identityServerClientFactory = new IdentityServerClientFactory();
            var identityServerUmaClientFactory = new IdentityServerUmaClientFactory();
            var documentManagementFactory = new DocumentManagementFactory();
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

            var decryptedResult = documentManagementFactory.GetOfficeDocumentClient().DecryptResolve(new DecryptDocumentRequest
            {
                DocumentId = sidDocumentIdValue,
                Credentials = credentials,
                Kid = kid
            }, Constants.DocumentApiConfiguration, grantedToken.AccessToken).Result;
            if (decryptedResult.ContainsError)
            {
                return null;
            }

            return encryptionHelper.Decrypt(encryptedContent, decryptedResult.Content);
        }

        private void HandleDisconnect(object sender, EventArgs e)
        {
            ClearCookie();
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;
            string sidDocumentIdValue;
            if (!activeDocument.TryGetVariable(Constants.VariableName, out sidDocumentIdValue))
            {
                return;
            }

            OfficeDocumentStore.Instance().ResetDecryption(sidDocumentIdValue);
        }

        private void InternalStartup()
        {            
            Application.DocumentBeforeClose += HandleDocumentBeforeClose;
            OfficeDocumentStore.Instance().Decrypted += HandleDecryptDocument;
            AuthenticationStore.Instance().Disconnected += HandleDisconnect;
        }

        private static void ClearCookie()
        {
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
        }

        private static void SetEncrypted(Document doc, string val)
        {
            string str;
            if (doc.TryGetVariable(Constants.IsEncryptedVariableName, out str))
            {
                var variable = doc.Variables[Constants.IsEncryptedVariableName];
                variable.Value = val;
                return;
            }

            doc.Variables.Add(Constants.IsEncryptedVariableName, val);
        }

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
    }
}
