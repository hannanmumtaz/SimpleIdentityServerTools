using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WordAccessManagementAddin.Controls.ViewModels
{
    internal sealed class PermissionViewModel : BaseViewModel
    {
        private string _name;
        private bool _isSelected;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = true;
                }
            }
        }
    }

    internal sealed class UserViewModel : BaseViewModel
    {
        private string _name;
        private IEnumerable<string> _permissions;
        private bool _isSelected;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = true;
                }
            }
        }

        public IEnumerable<string> Permissions
        {
            get
            {
                return _permissions;
            }
            set
            {
                if(_permissions != value)
                {
                    _permissions = value;
                }
            }
        }
    }

    internal sealed class ProtectUserViewModel : BaseViewModel
    {
        private string _userIdentifier;

        public ProtectUserViewModel()
        {
            Permissions = new ObservableCollection<PermissionViewModel>();
            Users = new ObservableCollection<UserViewModel>();
            AddCommand = new RelayCommand(HandleAddPermission, p => CanExecuteAddPermission());
            RemoveCommand = new RelayCommand(HandleRemovePermissions, p => CanExecuteRemovePermissions());
            SaveCommand = new RelayCommand(HandleSavePermissions, p => CanSavePermissions());
        }

        public string UserIdentifier
        {
            get
            {
                return _userIdentifier;
            }
            set
            {
                if (_userIdentifier != value)
                {
                    _userIdentifier = value;
                    OnPropertyChanged("UserIdentifier");
                }
            }
        }
        public ObservableCollection<PermissionViewModel> Permissions { get; set; }
        public ObservableCollection<UserViewModel> Users { get; set; }
        public ICommand AddCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public event EventHandler PermissionAdded;
        public event EventHandler PermissionsRemoved;
        public event EventHandler PermissionsSaved;

        private void HandleAddPermission(object o)
        {
            if (PermissionAdded != null)
            {
                PermissionAdded(this, EventArgs.Empty);
            }
        }

        private bool CanExecuteAddPermission()
        {
            return true;
        }

        private void HandleRemovePermissions(object o)
        {
            if (PermissionsRemoved != null)
            {
                PermissionsRemoved(this, EventArgs.Empty);
            }
        }

        private bool CanExecuteRemovePermissions()
        {
            return true;
        }

        private void HandleSavePermissions(object o)
        {
            if (PermissionsSaved != null)
            {
                PermissionsSaved(this, EventArgs.Empty);
            }
        }

        private bool CanSavePermissions()
        {
            return true;
        }
    }
}
