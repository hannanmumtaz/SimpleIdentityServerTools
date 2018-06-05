using System;
using System.Windows.Forms;

namespace WordAccessManagementAddin
{
    public partial class AuthenticateForm : Form
    {
        public AuthenticateForm()
        {
            InitializeComponent();
            webBrowser.Navigated += OnNavigated;
            Load += OnLoaded;
        }

        private void OnNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url == null || !e.Url.AbsoluteUri.Contains("http://localhost:64950"))
            {
                return;
            }

            var query = e.Url.Query;
            query = query.Replace("?", "");
            var splittedQuery = query.Split('&');
            foreach(var kvp in splittedQuery)
            {

            }
        }

        private void OnLoaded(object sender, System.EventArgs e)
        {
            webBrowser.Navigate(new Uri("http://localhost:60000/authorization?scope=openid role profile&state=75BCNvRlEGHpQRCT&redirect_uri=http://localhost:64950/callback&response_type=id_token token&client_id=ResourceManagerClientId&nonce=nonce&response_mode=query"));
        }
    }
}
