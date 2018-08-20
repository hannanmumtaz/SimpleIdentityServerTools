using SimpleIdentityServer.Client;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Core.Common;
using SimpleIdentityServer.DocumentManagement.Client;
using SimpleIdentityServer.DocumentManagement.Client.Responses;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using System;
using System.Linq;
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
            ViewModel.SharedLinkAdded += HandleAddSharedLink;
            ViewModel.SelectedSharedLinkRemoved += HandleRemoveSharedLink;
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

            ViewModel.IsDocumentProtected = true;
            var officeDocumentClient = _documentManagementFactory.GetOfficeDocumentClient();
            officeDocumentClient.GetResolve(sidDocumentIdValue, Constants.DocumentApiConfiguration, _authenticationStore.AccessToken).ContinueWith((dr) =>
            {
                var getDocumentResult = dr.Result;
                if (getDocumentResult.ContainsError)
                {
                    DisplayLoading(false);
                    return;
                }

                _window.Dispatcher.Invoke(new Action(() =>
                {
                    ViewModel.DisplayName = getDocumentResult.OfficeDocument.DisplayName;
                }));
                var subject = _authenticationStore.JwsPayload["sub"].ToString();
                if(getDocumentResult.OfficeDocument.Subject != subject)
                {
                    DisplayLoading(false);
                    return;
                }

                officeDocumentClient.GetAllInvitationLinksResolve(sidDocumentIdValue, Constants.DocumentApiConfiguration, _authenticationStore.AccessToken).ContinueWith((lr) =>
                {
                    var invitationLinksResult = lr.Result;
                    DisplayLoading(false);
                    if (invitationLinksResult.ContainsError)
                    {
                        DisplayErrorMessage("An error occured while trying to get the confirmation links");
                        return;
                    }

                    if (invitationLinksResult.Content != null)
                    {
                        foreach (var record in invitationLinksResult.Content)
                        {
                            _window.Dispatcher.Invoke(new Action(() =>
                            {
                                ViewModel.SharedLinks.Add(new SharedLinkViewModel
                                {
                                    ConfirmationCode = record.ConfirmationCode,
                                    IsSelected = false,
                                    RedirectUrl = record.RedirectUrl
                                });
                            }));
                        }
                    }
                }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith((lr) =>
                {
                    DisplayLoading(false);
                });
                // TODO : DISPLAY ALL THE USERS
            }, TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith((dr) =>
            {
                DisplayLoading(false);
            }, TaskContinuationOptions.OnlyOnFaulted);
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

            if(string.IsNullOrWhiteSpace(ViewModel.DisplayName))
            {

            }

            string sidDocumentIdValue;
            if (!activeDocument.TryGetVariable(Constants.VariableName, out sidDocumentIdValue))
            {
                DisplayLoading(true);
                sidDocumentIdValue = Guid.NewGuid().ToString();
                ProtectDocument(sidDocumentIdValue, ViewModel.DisplayName).ContinueWith((br) =>
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

        /// <summary>
        /// Create a shared link.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleAddSharedLink(object sender, EventArgs e)
        {
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;
            if (activeDocument == null)
            {
                return;
            }

            string sidDocumentIdValue;
            if (activeDocument.TryGetVariable(Constants.VariableName, out sidDocumentIdValue))
            {
                var request = new GenerateConfirmationCodeRequest
                {
                    ExpiresIn = ViewModel.IsExpiresInEnabled ? (int?)ViewModel.ExpiresIn : null,
                    NumberOfConfirmations = ViewModel.IsNumberOfDownloadsEnabled ? (int?)ViewModel.NumberOfDownloads : null
                };
                DisplayLoading(true);
                AddSharedLink(sidDocumentIdValue, request).ContinueWith((gi) =>
                {
                    var result = gi.Result;
                    DisplayLoading(false);
                    if (result.ContainsError)
                    {
                        DisplayErrorMessage("An error occured while trying to generate the shared link");
                        return;
                    }

                    _window.Dispatcher.Invoke(new Action(() =>
                    {
                        ViewModel.SharedLinks.Add(new SharedLinkViewModel
                        {
                            ConfirmationCode = result.Content.ConfirmationCode,
                            IsSelected = false,
                            RedirectUrl = result.Content.Url
                        });
                    }));
                    DisplayInformationMessage("The shared link has been generated");
                }, TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith((gi) =>
                {
                    DisplayErrorMessage("An error occured while trying to interact with the DocumentApi");
                    DisplayLoading(false);
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        /// <summary>
        /// Remove the shared link.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleRemoveSharedLink(object sender, EventArgs e)
        {
            var selectedSharedLink = ViewModel.SharedLinks.FirstOrDefault(s => s.IsSelected);
            if(selectedSharedLink == null)
            {
                return;
            }
            
            DisplayLoading(true);
            var officeDocumentClient = _documentManagementFactory.GetOfficeDocumentClient();
            officeDocumentClient.DeleteConfirmationLinkResolve(selectedSharedLink.ConfirmationCode, Constants.DocumentApiConfiguration, _authenticationStore.AccessToken).ContinueWith((br) =>
            {
                var result = br.Result;
                DisplayLoading(false);
                if(result.ContainsError)
                {
                    DisplayErrorMessage("An error occured while trying to remove the shared link");
                }

                _window.Dispatcher.Invoke(new Action(() =>
                {
                    ViewModel.SharedLinks.Remove(selectedSharedLink);
                }));
                DisplayInformationMessage("The selected shared link has been removed");
            }, TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith((br) =>
            {
                DisplayErrorMessage("An error occured while trying to interact with the DocumentApi");
                DisplayLoading(false);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private Task<BaseResponse> ProtectDocument(string documentId, string displayName)
        {
            var officeDocumentClient = _documentManagementFactory.GetOfficeDocumentClient();
            return officeDocumentClient.AddResolve(new AddOfficeDocumentRequest
            {
                Id = documentId,
                DisplayName = displayName
            }, Constants.DocumentApiConfiguration, _authenticationStore.AccessToken);
        }

        private Task<GetInvitationLinkResponse> AddSharedLink(string documentId, GenerateConfirmationCodeRequest request)
        {
            var officeDocumentClient = _documentManagementFactory.GetOfficeDocumentClient();
            return officeDocumentClient.GetInvitationLinkResolve(documentId, request, Constants.DocumentApiConfiguration, _authenticationStore.AccessToken);
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
