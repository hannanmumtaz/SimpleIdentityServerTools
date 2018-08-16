using System;
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

    internal sealed class ProtectUserViewModel : BaseViewModel
    {
        private string _userIdentifier;
        private bool _isLoading;
        private bool _isMessageDisplayed;
        private string _message;
        private bool _isErrorMessage;

        public ProtectUserViewModel()
        {
            Users = new ObservableCollection<UserViewModel>();
            AddCommand = new RelayCommand(HandleAddPermission, p => CanExecuteAddPermission());
            RemoveCommand = new RelayCommand(HandleRemovePermissions, p => HandleCanExecuteRemovePermissions());
            SaveCommand = new RelayCommand(HandleSavePermissions, p => CanSavePermissions());
            CloseMessageCommand = new RelayCommand(HandleCloseMessage, p => true);
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                if(_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
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
                    OnPropertyChanged(nameof(UserIdentifier));
                }
            }
        }
        public bool IsMessageDisplayed
        {
            get
            {
                return _isMessageDisplayed;
            }
            set
            {
                if(_isMessageDisplayed != value)
                {
                    _isMessageDisplayed = value;
                    OnPropertyChanged(nameof(IsMessageDisplayed));
                }
            }
        }
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }
        public bool IsErrorMessage
        {
            get
            {
                return _isErrorMessage;
            }
            set
            {
                if(_isErrorMessage != value)
                {
                    _isErrorMessage = value;
                    OnPropertyChanged(nameof(IsErrorMessage));
                }
            }
        }
        public ObservableCollection<UserViewModel> Users { get; set; }
        public ICommand AddCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CloseMessageCommand { get; private set; }
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

        private bool HandleCanExecuteRemovePermissions()
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

        private void HandleCloseMessage(object o)
        {
            Message = null;
            IsMessageDisplayed = false;
        }
    }
}
