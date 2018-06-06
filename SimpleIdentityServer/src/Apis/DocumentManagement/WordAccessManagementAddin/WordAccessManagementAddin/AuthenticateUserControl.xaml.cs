using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WordAccessManagementAddin.Stores;

namespace WordAccessManagementAddin
{
    public partial class AuthenticateUserControl : Window
    {
        private const string _clientId = "ResourceManagerClientId";
        private const string _callbackUrl = "http://localhost:64950/callback";
        private const string _baseUrl = "http://localhost:60000";

        public AuthenticateUserControl()
        {
            InitializeComponent();
            webBrowser.Navigate(new Uri($"{_baseUrl}/authorization?scope=openid role profile&state=75BCNvRlEGHpQRCT&redirect_uri={_callbackUrl}&response_type=id_token token&client_id={_clientId}&nonce=nonce&response_mode=query"));
            webBrowser.Navigating += HandleNavigating;
            webBrowser.Navigated += HandleNavigated;
        }

        private void HandleNavigating(object sender, NavigatingCancelEventArgs e)
        {
            DisplaySpinner(true);
        }

        private void HandleNavigated(object sender, NavigationEventArgs e)
        {
            HideScriptErrors(webBrowser, true);
            DisplaySpinner(false);
            if (e.Uri == null || !e.Uri.AbsoluteUri.StartsWith(_callbackUrl))
            {
                return;
            }

            var query = e.Uri.Query;
            query = query.Replace("?", "");
            var parameters = query.Split('&').Select(r =>
            {
                var splittedParameter = r.Split('=');
                return new KeyValuePair<string, string>(splittedParameter[0], splittedParameter[1]);
            });
            AuthenticationStore.Instance().Update(parameters);
            Close();
        }

        private void DisplaySpinner(bool isDisplayed)
        {
            if (isDisplayed)
            {
                spinner.Visibility = Visibility.Visible;
                webBrowser.Visibility = Visibility.Collapsed;
            }
            else
            {
                spinner.Visibility = Visibility.Collapsed;
                webBrowser.Visibility = Visibility.Visible;
            }
        }

        public void HideScriptErrors(WebBrowser wb, bool Hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            object objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null) return;
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        }
    }
}
