using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using WordAccessManagementAddin.Stores;

namespace WordAccessManagementAddin
{
    public partial class Ribbon
    {
        private AuthenticationStore _authenticationStore;
        private const string _password = "VerySecret!";

        private void HandleRibbonLoad(object sender, RibbonUIEventArgs e)
        {
            _authenticationStore = AuthenticationStore.Instance();
            _authenticationStore.Authenticated += HandleAuthenticate;
        }

        private void HandleAuthenticate(object sender, EventArgs e)
        {
            var jwsPayload = _authenticationStore.JwsPayload;
            var givenName = jwsPayload["given_name"];
            var picture = jwsPayload["picture"];
            string s = "";
        }

        private void HandleLogin(object sender, RibbonControlEventArgs e)
        {
            var authForm = new AuthenticateUserControl();
            authForm.Show();
        }

        private void HandleProtect(object sender, RibbonControlEventArgs e)
        {
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;
            using (var aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;
                var keyStrengthInBytes = aes.KeySize / 8;
                var rfc2898 = new Rfc2898DeriveBytes(_password, new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 }, 100);
                aes.Key = rfc2898.GetBytes(keyStrengthInBytes);
                aes.IV = rfc2898.GetBytes(keyStrengthInBytes);
                foreach (Section section in activeDocument.Sections)
                {
                    Range range = section.Range;
                    // EXTRACT THE IMAGES.
                    /*
                    foreach(Microsoft.Office.Interop.Word.InlineShape shape in range.InlineShapes)
                    {
                        if (shape == null || shape.Type != WdInlineShapeType.wdInlineShapePicture)
                        {
                            return;
                        }
                        var s = shape.Range.Start;
                        var end = shape.Range.End;
                        string s1 = "";

                    }
                    */
                    var encryptedResult = new List<string>();
                    int nbParag = range.Paragraphs.Count;
                    for (int i = 0; i < nbParag; i++)
                    {
                        var paragraph = range.Paragraphs[i + 1];
                        var paraTxt = paragraph.Range.Text.Replace("\r", "");
                        var payload = Encoding.UTF8.GetBytes(paraTxt);
                        byte[] cipherText = null;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(payload, 0, payload.Length);
                            }

                            cipherText = ms.ToArray();
                        }

                        var b64Str = Convert.ToBase64String(cipherText);
                        if (i == nbParag - 1)
                        {
                            paragraph.Range.Text = b64Str;
                            continue;
                        }

                        paragraph.Range.Text = $"{b64Str}\r";
                    }
                }
            }
        }

        private void HandleUnprotect(object sender, RibbonControlEventArgs e)
        {
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;

            using (var aes = new AesManaged())
            {
                foreach (Section section in activeDocument.Sections)
                {
                    aes.Padding = PaddingMode.PKCS7;
                    aes.KeySize = 128;
                    var keyStrengthInBytes = aes.KeySize / 8;
                    var rfc2898 = new Rfc2898DeriveBytes(_password, new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 }, 100);
                    aes.Key = rfc2898.GetBytes(keyStrengthInBytes);
                    aes.IV = rfc2898.GetBytes(keyStrengthInBytes);
                    Range range = section.Range;
                    int nbParag = range.Paragraphs.Count;
                    for (int i = 0; i < nbParag; i++)
                    {
                        var paragraph = range.Paragraphs[i + 1];
                        var pTxt = paragraph.Range.Text.Replace("\r", "");
                        var payload = Convert.FromBase64String(pTxt);
                        byte[] cipherText = null;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(payload, 0, payload.Length);
                            }

                            cipherText = ms.ToArray();
                        }

                        var plainText = Encoding.UTF8.GetString(cipherText);
                        if (i == nbParag - 1)
                        {
                            paragraph.Range.Text = plainText;
                            continue;
                        }

                        paragraph.Range.Text = $"{plainText}\r";
                    }
                }
            }
        }
    }
}
