using ElectronNET.API;
using ElectronNET.API.Entities;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Eid.UI.Stores
{
    public interface IUiStore
    {
        Task Display();
        void Show();
        void Hide();
    }

    internal sealed class UiStore : IUiStore
    {
        private BrowserWindow _browserWindow;

        public async Task Display()
        {
            if (_browserWindow != null)
            {
                throw new InvalidOperationException("Already displayed");
            }

            _browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 200,
                Height = 200,
                Show = false
            });
            _browserWindow.SetClosable(false);
            _browserWindow.OnMinimize += () => { _browserWindow.Hide(); };
            _browserWindow.OnReadyToShow += () => { _browserWindow.Show(); };
            var openMenuItem = new MenuItem
            {
                Label = "Ouvrir",
                Click = () =>
                {
                    _browserWindow.Show();
                }
            };
            var closeMenuItem = new MenuItem
            {
                Label = "Fermer",
                Click = () =>
                {
                    _browserWindow.Destroy();
                    Electron.Tray.Destroy();
                }
            };
            Electron.Tray.Show("/Assets/electron_32x32.png", new[] { openMenuItem, closeMenuItem });

        }

        public void Show()
        {
            if (_browserWindow == null)
            {
                throw new InvalidOperationException("The window is not displayed");
            }

            _browserWindow.Show();
        }

        public void Hide()
        {
            if (_browserWindow == null)
            {
                throw new InvalidOperationException("The window is not displayed");
            }

            _browserWindow.Hide();
        }
    }
}
