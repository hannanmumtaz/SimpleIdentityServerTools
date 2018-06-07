using SimpleIdentityServer.Core.Common;
using System.IO;
using WordAccessManagementAddin.Controls.ViewModels;
using WordAccessManagementAddin.Stores;

namespace WordAccessManagementAddin.Controls.Controllers
{
    internal sealed class ProfileUserController
    {
        private readonly AuthenticationStore _store;

        public ProfileUserController()
        {
            ViewModel = new ProfileUserViewModel();
            ViewModel.WindowLoaded += HandleWindowLoaded;
            _store = AuthenticationStore.Instance();
            _store.Authenticated += HandleAuthenticate;
        }

        public ProfileUserViewModel ViewModel { get; private set; }

        private void HandleWindowLoaded(object sender, System.EventArgs e)
        {
            UpdateViewModel();
        }

        private void HandleAuthenticate(object sender, System.EventArgs e)
        {
            UpdateViewModel();
        }

        /// <summary>
        /// Update the view model informations.
        /// </summary>
        private void UpdateViewModel()
        {
            if (_store.JwsPayload == null)
            {
                return;
            }

            var gender = TryGetKey(_store.JwsPayload, SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Gender);
            var picture = TryGetKey(_store.JwsPayload, SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Picture);
            if (string.IsNullOrWhiteSpace(picture))
            {
                if (gender == "female")
                {
                    picture =  "WordAccessManagementAddin.Resources.female.png";
                }
                else
                {
                    picture = "WordAccessManagementAddin.Resources.male.png";
                }
            }

            ViewModel.Identifier = TryGetKey(_store.JwsPayload, SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject);
            ViewModel.GivenName = TryGetKey(_store.JwsPayload, SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.GivenName);
            ViewModel.Picture = picture;
        }

        /// <summary>
        /// Try to get the value from the PAYLOAD.
        /// </summary>
        /// <param name="jwsPayload"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string TryGetKey(JwsPayload jwsPayload, string key)
        {
            if (!jwsPayload.ContainsKey(key))
            {
                return string.Empty;
            }

            return jwsPayload[key].ToString();
        }
    }
}