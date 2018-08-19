using SimpleIdentityServer.Client;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Core.Common;
using SimpleIdentityServer.DocumentManagement.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using System;
using System.Threading.Tasks;
using System.Windows;
using WordAccessManagementAddin.Controls.ViewModels;
using WordAccessManagementAddin.Extensions;
using WordAccessManagementAddin.Stores;

namespace WordAccessManagementAddin.Controls.Controllers
{
    internal sealed class ProtectUserController
    {
        private readonly DocumentManagementFactory _documentManagementFactory;
        private readonly IdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IdentityServerClientFactory _identityServerClientFactory;
        private readonly AuthenticationStore _authenticationStore;
        private readonly OfficeDocumentStore _officeDocumentStore;
        private readonly Window _window;

        public ProtectUserController(Window window)
        {
            _window = window;
            _documentManagementFactory = new DocumentManagementFactory();
            _identityServerUmaClientFactory = new IdentityServerUmaClientFactory();
            _identityServerClientFactory = new IdentityServerClientFactory();
            _authenticationStore = AuthenticationStore.Instance();
            _officeDocumentStore = OfficeDocumentStore.Instance();
            ViewModel = new ProtectUserViewModel();
            Init();
            ViewModel.DocumentProtected += HandleProtectDocument;
        }

        /// <summary>
        /// Initialize the module.
        /// </summary>
        private void Init()
        {
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;
            if (activeDocument == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(_authenticationStore.IdentityToken))
            {
                return;
            }

            DisplayLoading(true);
            string sidDocumentIdValue;
            if (!activeDocument.TryGetVariable(Constants.VariableName, out sidDocumentIdValue))
            {
                ViewModel.IsDocumentProtected = false;
                DisplayLoading(false);
                return;
            }

            DisplayLoading(false);
            ViewModel.IsDocumentProtected = true;
            // TODO : DISPLAY THE PERMISSIONS
            // TODO : DISPLAY THE SHARED LINKS
            /*
            GetPermissions(sidDocumentIdValue).ContinueWith((r) =>
            {
                var permissions = r.Result;
                DisplayLoading(false);
                if (permissions == null || permissions.ContainsError)
                {
                    return;
                }

                var sub = TryGetKey(_authenticationStore.JwsPayload, SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject);
                foreach(var permission in permissions.Content.Where(c => c.UserSubject != sub))
                {
                    _window.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                        ViewModel.Users.Add(new UserViewModel
                        {
                            IsSelected = false,
                            Name = permission.UserSubject
                        })
                    ));
                }
            });
            */
        }

        /// <summary>
        /// Protect the document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleProtectDocument(object sender, EventArgs e)
        {
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;
            if (activeDocument == null)
            {
                return;
            }

            string sidDocumentIdValue;
            if (!activeDocument.TryGetVariable(Constants.VariableName, out sidDocumentIdValue))
            {
                DisplayLoading(true);
                sidDocumentIdValue = Guid.NewGuid().ToString();
                ProtectDocument(sidDocumentIdValue).ContinueWith((br) =>
                {
                    var result = br.Result;
                    DisplayLoading(false);
                    if (result.ContainsError)
                    {
                        DisplayErrorMessage("An error occured while trying to protect the document");
                        return;
                    }

                    DisplayInformationMessage("The document is now protected");
                    ViewModel.IsDocumentProtected = true;
                    activeDocument.Variables.Add(Constants.VariableName, sidDocumentIdValue);
                    activeDocument.Save();
                }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith((br) =>
                {
                    DisplayErrorMessage("An error occured while trying to interact with the DocumentApi");
                    DisplayLoading(false);
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private Task<BaseResponse> ProtectDocument(string documentId)
        {
            var officeDocumentClient = _documentManagementFactory.GetOfficeDocumentClient();
            return officeDocumentClient.AddResolve(new AddOfficeDocumentRequest
            {
                Id = documentId
            }, Constants.DocumentApiConfiguration, _authenticationStore.AccessToken);
        }

        /*
        private async Task<GetOfficeDocumentPermissionsResponse> GetPermissions(string documentId)
        {
            var umaResourceId = await _officeDocumentStore.GetUmaResourceId(documentId).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(umaResourceId))
            {
                return null;
            }

            var grantedToken = await _officeDocumentStore.GetOfficeDocumentAccessTokenViaUmaGrantType(umaResourceId);
            if (grantedToken == null)
            {
                return null;
            }

            return await _documentManagementFactory.GetOfficeDocumentClient().GetPermissionsResolve(documentId, Constants.DocumentApiConfiguration, grantedToken.AccessToken).ConfigureAwait(false);
        }
        */

        /// <summary>
        /// Save the permissions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*
        private async Task HandlePermissionsSaved()
        {
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;
            if (activeDocument == null)
            {
                return;
            }

            DisplayLoading(true);
            var officeDocumentClient = _documentManagementFactory.GetOfficeDocumentClient();
            string sidDocumentIdValue;
            if (!activeDocument.TryGetVariable(Constants.VariableName, out sidDocumentIdValue))
            {
                sidDocumentIdValue = Guid.NewGuid().ToString();
                try
                {
                    var addResponse = await officeDocumentClient.AddResolve(new AddOfficeDocumentRequest
                    {
                        Id = sidDocumentIdValue
                    }, Constants.DocumentApiConfiguration, _authenticationStore.AccessToken);
                    if (addResponse.ContainsError)
                    {
                        DisplayErrorMessage("An error occured while trying to interact with the DocumentApi");
                        DisplayLoading(false);
                        return;
                    }
                }
                catch(Exception)
                {
                    DisplayErrorMessage("An error occured while trying to interact with the DocumentApi");
                    DisplayLoading(false);
                    return;
                }
                
                activeDocument.Variables.Add(Constants.VariableName, sidDocumentIdValue);
            }

            var permissions = new List<OfficeDocumentPermissionRequest>();
            if (ViewModel.Users != null && ViewModel.Users.Any())
            {
                foreach(var user in ViewModel.Users)
                {
                    permissions.Add(new OfficeDocumentPermissionRequest
                    {
                        Scopes = new List<string>
                        {
                            "read"
                        },
                        Subject = user.Name
                    });
                }
            }

            if (!permissions.Any())
            {
                DisplayErrorMessage("At least one permission must be inserted");
                DisplayLoading(false);
                return;
            }

            try
            {
                var updateResult = await officeDocumentClient.UpdateResolve(sidDocumentIdValue, new UpdateOfficeDocumentRequest
                {
                    Permissions = permissions
                }, Constants.DocumentApiConfiguration, _authenticationStore.AccessToken);
                if (updateResult.ContainsError)
                {
                    DisplayErrorMessage("An error occured while trying to update the permissions");
                    DisplayLoading(false);
                    return;
                }
            }
            catch(Exception)
            {
                DisplayErrorMessage("An error occured while trying to update the permissions");
                DisplayLoading(false);
                return;
            }

            DisplayInformationMessage("the permissions are saved");
            DisplayLoading(false);
        }
        */

        /// <summary>
        /// Remove the selected users.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*
        private void HandlePermissionsRemoved(object sender, System.EventArgs e)
        {
            var selectedUsers = ViewModel.Users.Where(p => p.IsSelected);
            if (!selectedUsers.Any())
            {
                DisplayErrorMessage("At least one user must be selected");
                return;
            }

            var names = selectedUsers.Select(u => u.Name).ToList();
            foreach(var name in names)
            {
                ViewModel.Users.Remove(ViewModel.Users.First(u => u.Name == name));
            }
        }
        */

        /// <summary>
        /// Add a permission.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*
        private void HandleAddPermission(object sender, System.EventArgs e)
        {
            var userIdentifier = ViewModel.UserIdentifier;
            if (string.IsNullOrWhiteSpace(userIdentifier))
            {
                DisplayErrorMessage("The user identifier is mandatory");
                return;
            }

            ViewModel.UserIdentifier = string.Empty;
            ViewModel.Users.Add(new UserViewModel
            {
                IsSelected = false,
                Name = userIdentifier
            });
        }
        */

        private void DisplayErrorMessage(string errorMessage)
        {
            ViewModel.Message = errorMessage;
            ViewModel.IsMessageDisplayed = true;
            ViewModel.IsErrorMessage = true;
        }

        private void DisplayInformationMessage(string infoMessage)
        {
            ViewModel.Message = infoMessage;
            ViewModel.IsMessageDisplayed = true;
            ViewModel.IsErrorMessage = false;
        }
        
        private void DisplayLoading(bool isDisplayed)
        {
            ViewModel.IsLoading = isDisplayed;
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

        public ProtectUserViewModel ViewModel { get; set; }
    }
}
