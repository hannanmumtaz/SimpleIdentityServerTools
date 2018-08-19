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
        private bool _isDocumentProtected;
        private bool _isLoading;
        private bool _isMessageDisplayed;
        private string _message;
        private bool _isErrorMessage;
        private string _displayName;

        public ProtectUserViewModel()
        {
            Users = new ObservableCollection<UserViewModel>();
            ProtectDocumentCommand = new RelayCommand(HandleProtectDocument, p => CanExecuteProtectDocumentCommand());
            CloseMessageCommand = new RelayCommand(HandleCloseMessage, p => true);
        }

        public bool IsDocumentProtected
        {
            get
            {
                return _isDocumentProtected;
            }
            set
            {
                if(_isDocumentProtected != value)
                {
                    _isDocumentProtected = value;
                    OnPropertyChanged(nameof(IsDocumentProtected));
                }
            }
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
        public bool IsErrorMessage
        {
            get
            {
                return _isErrorMessage;
            }
            set
            {
                if (_isErrorMessage != value)
                {
                    _isErrorMessage = value;
                    OnPropertyChanged(nameof(IsErrorMessage));
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
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if(_displayName != value)
                {
                    _displayName = value;
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }
        public ObservableCollection<UserViewModel> Users { get; set; }
        public ICommand ProtectDocumentCommand { get; private set; }
        public ICommand CloseMessageCommand { get; private set; }
        public event EventHandler DocumentProtected;

        private void HandleProtectDocument(object o)
        {
            if (DocumentProtected != null)
            {
                DocumentProtected(this, EventArgs.Empty);
            }
        }

        private bool CanExecuteProtectDocumentCommand()
        {
            return !IsDocumentProtected;
        }

        private void HandleCloseMessage(object o)
        {
            Message = null;
            IsMessageDisplayed = false;
        }
    }
}
