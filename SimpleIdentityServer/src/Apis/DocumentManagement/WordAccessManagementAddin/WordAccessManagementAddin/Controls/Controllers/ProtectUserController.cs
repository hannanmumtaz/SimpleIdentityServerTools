using System.Linq;
using WordAccessManagementAddin.Controls.ViewModels;

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
            ViewModel.PermissionsSaved += HandlePermissionsSaved;
        }

        /// <summary>
        /// Save the permissions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlePermissionsSaved(object sender, System.EventArgs e)
        {

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
