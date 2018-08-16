using System.Windows;
using WordAccessManagementAddin.Controls.Controllers;

namespace WordAccessManagementAddin.Controls
{
    public partial class ProfileUserControl : Window
    {
        private readonly ProfileUserController _controller;

        public ProfileUserControl()
        {
            InitializeComponent();
            _controller = new ProfileUserController();
            DataContext = _controller.ViewModel;
        }
    }
}
