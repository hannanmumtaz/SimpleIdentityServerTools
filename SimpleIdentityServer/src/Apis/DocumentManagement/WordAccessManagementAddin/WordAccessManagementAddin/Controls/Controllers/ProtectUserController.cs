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
            ViewModel.Permissions.Add(new PermissionViewModel
            {
                IsSelected = false,
                Name = "read"
            });
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
            ViewModel.CanExecuteRemovePermissions = false;
            var activeDocument = Globals.ThisAddIn.Application.ActiveDocument;
            if (activeDocument == null)
            {
                return;
            }

            var docMgClientFactory = new DocumentManagementFactory();
            var officeDocumentClient = docMgClientFactory.GetOfficeDocumentClient();
            var authenticateStore = AuthenticationStore.Instance();
            string sidDocumentIdValue;
            if (!activeDocument.TryGetVariable(Constants.VariableName, out sidDocumentIdValue))
            {
                sidDocumentIdValue = Guid.NewGuid().ToString();
                var addResponse = await officeDocumentClient.AddResolve(new AddOfficeDocumentRequest
                {
                    Id = sidDocumentIdValue
                }, Constants.DocumentApiConfiguration, authenticateStore.AccessToken);
                if (addResponse.ContainsError)
                {
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
                        Scopes = user.Permissions,
                        Subject = user.Name
                    });
                }
            }

            if (!permissions.Any())
            {
                return;
            }

            await officeDocumentClient.UpdateResolve(sidDocumentIdValue, new UpdateOfficeDocumentRequest
            {
                Permissions = permissions
            }, Constants.DocumentApiConfiguration, authenticateStore.AccessToken);
            ViewModel.CanExecuteRemovePermissions = true;
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
            var selectedPermissions = ViewModel.Permissions.Where(p => p.IsSelected);
            if (string.IsNullOrWhiteSpace(userIdentifier) || !selectedPermissions.Any() || ViewModel.Users.Any(u => u.Name == userIdentifier))
            {
                return;
            }

            ViewModel.UserIdentifier = string.Empty;
            ViewModel.Users.Add(new UserViewModel
            {
                IsSelected = false,
                Name = userIdentifier,
                Permissions = selectedPermissions.Select(s => s.Name)
            });
            foreach(var permission in ViewModel.Permissions)
            {
                permission.IsSelected = false;
            }
        }

        public ProtectUserViewModel ViewModel { get; set; }
    }
}
