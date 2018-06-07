using System;
using System.Windows.Input;

namespace WordAccessManagementAddin.Controls.ViewModels
{
    internal sealed class ProfileUserViewModel : BaseViewModel
    {
        private string _identifier;
        private string _picture;
        private string _givenName;
        private ICommand _loadCommand;

        public ProfileUserViewModel()
        {
            _loadCommand = new RelayCommand(LoadCommandExecute, p => CanExecuteLoadCommand());
        }

        public event EventHandler WindowLoaded;

        public string GivenName
        {
            get
            {
                return _givenName;
            }
            set
            {
                if(_givenName != value)
                {
                    _givenName = value;
                    OnPropertyChanged("GivenName");
                }
            }
        }

        public string Identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                if (_identifier != value)
                {
                    _identifier = value;
                    OnPropertyChanged("Identifier");
                }
            }
        }

        public string Picture
        {
            get
            {
                return _picture;
            }
            set
            {
                if (_picture != value)
                {
                    _picture = value;
                    OnPropertyChanged("Picture");
                }
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                return _loadCommand;
            }
            private set
            {
                _loadCommand = value;
            }
        }

        private void LoadCommandExecute(object o)
        {
            if (WindowLoaded != null)
            {
                WindowLoaded(this, EventArgs.Empty);
            }
        }

        private bool CanExecuteLoadCommand()
        {
            return true;
        }
    }
}
