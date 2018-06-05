using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WordAccessManagementAddin
{
    public partial class Ribbon
    {
        private const string _password = "VerySecret!";
        private AesManaged _aes;

        private void OnLoadRibbon(object sender, RibbonUIEventArgs e)
        {
            _aes = new AesManaged();
            _aes.Padding = PaddingMode.PKCS7;
            _aes.KeySize = 128;
            var keyStrengthInBytes = _aes.KeySize / 8;
            var rfc2898 = new Rfc2898DeriveBytes(_password, new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 }, 100);
            _aes.Key = rfc2898.GetBytes(keyStrengthInBytes);
            _aes.IV = rfc2898.GetBytes(keyStrengthInBytes);
        }

        private void OnClickLogin(object sender, RibbonControlEventArgs e)
        {
            var authForm = new AuthenticateForm();
            authForm.Show();
        }

        private void OnProtect(object sender, RibbonControlEventArgs e)
        {
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;
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
                        using (CryptoStream cs = new CryptoStream(ms, _aes.CreateEncryptor(), CryptoStreamMode.Write))
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

        private void OnUnprotect(object sender, RibbonControlEventArgs e)
        {
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;
            foreach (Section section in activeDocument.Sections)
            {
                Range range = section.Range;
                int nbParag = range.Paragraphs.Count;
                for(int i = 0; i < nbParag; i++)
                {
                    var paragraph = range.Paragraphs[i + 1];
                    var pTxt = paragraph.Range.Text.Replace("\r", "");
                    var payload = Convert.FromBase64String(pTxt);
                    byte[] cipherText = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, _aes.CreateDecryptor(), CryptoStreamMode.Write))
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
