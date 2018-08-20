using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WordAccessManagementAddin.Controls.ViewModels
{
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

    internal sealed class SharedLinkViewModel : BaseViewModel
    {
        private string _confirmationCode;
        private bool _isSelected;

        public SharedLinkViewModel()
        {
            CopyLinkCommand = new RelayCommand(HandleCopyLink, p => CanExecuteCopyLinkCommand());
        }

        public string ConfirmationCode
        {
            get
            {
                return _confirmationCode;
            }
            set
            {
                if(_confirmationCode != value)
                {
                    _confirmationCode = value;
                    OnPropertyChanged(nameof(ConfirmationCode));
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
                if(_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        public string RedirectUrl { get; set; }
        public ICommand CopyLinkCommand { get; }

        private void HandleCopyLink(object o)
        {
            System.Windows.Clipboard.SetText(RedirectUrl);
        }

        private bool CanExecuteCopyLinkCommand()
        {
            return true;
        }
    }

    internal sealed class ProtectUserViewModel : BaseViewModel
    {
        private bool _isDocumentProtected;
        private bool _isMessageDisplayed;
        private bool _isExpiresInEnabled;
        private bool _isNumberOfDownloadsEnabled;
        private bool _isLoading;
        private bool _isErrorMessage;
        private string _message;
        private string _displayName;
        private int _expiresIn;
        private int _numberOfDownloads;

        public ProtectUserViewModel()
        {
            Users = new ObservableCollection<UserViewModel>();
            SharedLinks = new ObservableCollection<SharedLinkViewModel>();
            ProtectDocumentCommand = new RelayCommand(HandleProtectDocument, p => CanExecuteProtectDocumentCommand());
            AddSharedLinkCommand = new RelayCommand(HandleAddSharedLink, p => CanExecuteAddSharedLinkCommand());
            RemoveSelectedSharedLinkCommand = new RelayCommand(HandleRemoveSelectedSharedLink, p => CanExecuteRemoveSelectedSharedLink());
            CloseMessageCommand = new RelayCommand(HandleCloseMessage, p => true);
            IsExpiresInEnabled = false;
            IsNumberOfDownloadsEnabled = false;
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
        public bool IsExpiresInEnabled
        {
            get
            {
                return _isExpiresInEnabled;
            }
            set
            {
                if(_isExpiresInEnabled != value)
                {
                    _isExpiresInEnabled = value;
                    OnPropertyChanged(nameof(IsExpiresInEnabled));
                }
            }
        }
        public bool IsNumberOfDownloadsEnabled
        {
            get
            {
                return _isNumberOfDownloadsEnabled;
            }
            set
            {
                if(_isNumberOfDownloadsEnabled != value)
                {
                    _isNumberOfDownloadsEnabled = value;
                    OnPropertyChanged(nameof(IsNumberOfDownloadsEnabled));
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
        public int ExpiresIn
        {
            get
            {
                return _expiresIn;
            }
            set
            {
                if(_expiresIn != value)
                {
                    _expiresIn = value;
                    OnPropertyChanged(nameof(ExpiresIn));
                }
            }
        }
        public int NumberOfDownloads
        {
            get
            {
                return _numberOfDownloads;
            }
            set
            {
                if(_numberOfDownloads != value)
                {
                    _numberOfDownloads = value;
                    OnPropertyChanged(nameof(NumberOfDownloads));
                }
            }
        }
        public ObservableCollection<UserViewModel> Users { get; set; }
        public ObservableCollection<SharedLinkViewModel> SharedLinks { get; set; }
        public ICommand ProtectDocumentCommand { get; private set; }
        public ICommand AddSharedLinkCommand { get; private set; }
        public ICommand RemoveSelectedSharedLinkCommand { get; private set; }
        public ICommand CloseMessageCommand { get; private set; }
        public event EventHandler DocumentProtected;
        public event EventHandler SharedLinkAdded;
        public event EventHandler SelectedSharedLinkRemoved;

        private void HandleRemoveSelectedSharedLink(object o)
        {
            if(SelectedSharedLinkRemoved != null)
            {
                SelectedSharedLinkRemoved(this, EventArgs.Empty);
            }
        }

        private bool CanExecuteRemoveSelectedSharedLink()
        {
            return true;
        }

        private void HandleProtectDocument(object o)
        {
            if (DocumentProtected != null)
            {
                DocumentProtected(this, EventArgs.Empty);
            }
        }

        private void HandleAddSharedLink(object o)
        {
            if(SharedLinkAdded != null)
            {
                SharedLinkAdded(this, EventArgs.Empty);
            }
        }

        private bool CanExecuteProtectDocumentCommand()
        {
            return !IsDocumentProtected;
        }

        private bool CanExecuteAddSharedLinkCommand()
        {
            return IsDocumentProtected;
        }

        private void HandleCloseMessage(object o)
        {
            Message = null;
            IsMessageDisplayed = false;
        }
    }
}
