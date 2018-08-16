using System.Windows;
using WordAccessManagementAddin.Controls.Controllers;

namespace WordAccessManagementAddin.Controls
{
    public partial class ProtectUserControl : Window
    {
        private readonly ProtectUserController _controller;

        public ProtectUserControl()
        {
            InitializeComponent();
            _controller = new ProtectUserController();
            DataContext = _controller.ViewModel;
        }
    }
}
