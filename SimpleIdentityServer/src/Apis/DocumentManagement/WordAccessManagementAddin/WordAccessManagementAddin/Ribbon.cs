using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using WordAccessManagementAddin.Controls;
using WordAccessManagementAddin.Helpers;
using WordAccessManagementAddin.Stores;

namespace WordAccessManagementAddin
{
    public partial class Ribbon
    {
        private AuthenticationStore _authenticationStore;

        private void HandleRibbonLoad(object sender, RibbonUIEventArgs e)
        {
            DisplayLogin(true);
            _authenticationStore = AuthenticationStore.Instance();
            _authenticationStore.Authenticated += HandleAuthenticate;
        }

        private void HandleAuthenticate(object sender, EventArgs e)
        {
            DisplayLogin(false);
        }

        private void HandleLogin(object sender, RibbonControlEventArgs e)
        {
            var authenticateUc = new AuthenticateUserControl();
            authenticateUc.Show();
        }

        private void HandleDisconnect(object sender, RibbonControlEventArgs e)
        {
            _authenticationStore.Disconnect();
            DisplayLogin(true);
        }

        private void HandleProtect(object sender, RibbonControlEventArgs e)
        {
            var protectUc = new ProtectUserControl();
            protectUc.Show();
        }

        private void HandleUnprotect(object sender, RibbonControlEventArgs e)
        {

        }

        private void HandleProtectOffline(object sender, RibbonControlEventArgs e)
        {
            var document = Globals.ThisAddIn.Application.ActiveDocument;
            var range = document.Range();
            var xml = range.XML;
            var image = ResourceHelper.GetImage("WordAccessManagementAddin.Resources.lock.png");
            var filePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
            var bm = SteganographyHelper.CreateNonIndexedImage(image);
            bm.Save(filePath);
            range.Text = string.Empty;
            var shape = range.InlineShapes.AddPicture(filePath, false, true);
            shape.AlternativeText = xml;
            File.Delete(filePath);
        }

        private void HandleUnprotectOffline(object sender, RibbonControlEventArgs e)
        {
            var document = Globals.ThisAddIn.Application.ActiveDocument;
            var range = document.Range();
            var shapes = range.InlineShapes;
            foreach(InlineShape shape in shapes)
            {
                if (shape.Type != WdInlineShapeType.wdInlineShapePicture || string.IsNullOrWhiteSpace(shape.AlternativeText))
                {
                    continue;
                }

                var xml = shape.AlternativeText;
                shape.Range.InsertXML(xml);
            }
        }

        private void HandleProfile(object sender, RibbonControlEventArgs e)
        {
            var profileUc = new ProfileUserControl();
            profileUc.Show();
        }

        private void DisplayLogin(bool isDisplayed)
        {
            profileButton.Visible = !isDisplayed;
            protectButton.Visible = !isDisplayed;
            unprotectButton.Visible = !isDisplayed;
            disconnectButton.Visible = !isDisplayed;
            loginButton.Visible = isDisplayed;
        }

        private static Bitmap GetBitmap(BitmapSource source)
        {
            var bmp = new Bitmap(source.PixelWidth, source.PixelHeight, PixelFormat.Format32bppPArgb);
            var data = bmp.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }
    }
}
