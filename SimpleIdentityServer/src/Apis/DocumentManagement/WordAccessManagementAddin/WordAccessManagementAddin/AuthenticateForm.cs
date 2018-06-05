using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WordAccessManagementAddin.Stores;

namespace WordAccessManagementAddin
{
    public partial class AuthenticateForm : Form
    {
        private const string _clientId = "ResourceManagerClientId";
        private const string _callbackUrl = "http://localhost:64950/callback";
        private const string _baseUrl = "http://localhost:60000";

        public AuthenticateForm()
        {
            InitializeComponent();
            webBrowser.Navigated += OnNavigated;
            Load += OnLoaded;
        }

        /// <summary>
        /// Get the access + identity tokens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url == null || !e.Url.AbsoluteUri.Contains(_callbackUrl))
            {
                return;
            }

            var query = e.Url.Query;
            query = query.Replace("?", "");
            var parameters = query.Split('&').Select(r =>
            {
                var splittedParameter = r.Split('=');
                return new KeyValuePair<string, string>(splittedParameter[0], splittedParameter[1]);
            });
            AuthenticationStore.ParseDictionary(parameters);
            Close();
        }

        /// <summary>
        /// Navigate to the authorization URL.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoaded(object sender, System.EventArgs e)
        {
            webBrowser.Navigate(new Uri($"{_baseUrl }/authorization?scope=openid role profile&state=75BCNvRlEGHpQRCT&redirect_uri={_callbackUrl}&response_type=id_token token&client_id={_clientId}&nonce=nonce&response_mode=query"));
        }
    }
}
