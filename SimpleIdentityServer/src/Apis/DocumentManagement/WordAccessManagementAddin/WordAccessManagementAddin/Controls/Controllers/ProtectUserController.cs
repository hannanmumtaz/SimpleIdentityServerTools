using SimpleIdentityServer.DocumentManagement.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordAccessManagementAddin.Controls.ViewModels;
using WordAccessManagementAddin.Extensions;
using WordAccessManagementAddin.Stores;

namespace WordAccessManagementAddin.Controls.Controllers
{
    internal sealed class ProtectUserController
    {
        public ProtectUserController()
        {
            ViewModel = new ProtectUserViewModel();
            ViewModel.PermissionAdded += HandleAddPermission;
            ViewModel.PermissionsRemoved += HandlePermissionsRemoved;
            ViewModel.PermissionsSaved += (s, e) => HandlePermissionsSaved();
        }

        /// <summary>
        /// Save the permissions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task HandlePermissionsSaved()
        {
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;
            if (activeDocument == null)
            {
                return;
            }

            DisplayLoading(true);
            var docMgClientFactory = new DocumentManagementFactory();
            var officeDocumentClient = docMgClientFactory.GetOfficeDocumentClient();
            var authenticateStore = AuthenticationStore.Instance();
            string sidDocumentIdValue;
            if (!activeDocument.TryGetVariable(Constants.VariableName, out sidDocumentIdValue))
            {
                sidDocumentIdValue = Guid.NewGuid().ToString();
                try
                {
                    var addResponse = await officeDocumentClient.AddResolve(new AddOfficeDocumentRequest
                    {
                        Id = sidDocumentIdValue
                    }, Constants.DocumentApiConfiguration, authenticateStore.AccessToken);
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
                }, Constants.DocumentApiConfiguration, authenticateStore.AccessToken);
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

        /// <summary>
        /// Remove the selected users.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Add a permission.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        public ProtectUserViewModel ViewModel { get; set; }
    }
}
