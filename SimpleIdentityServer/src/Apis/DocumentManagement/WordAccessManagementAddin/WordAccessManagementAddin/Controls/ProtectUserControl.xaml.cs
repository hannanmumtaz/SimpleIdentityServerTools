using System.Windows;
using WordAccessManagementAddin.Controls.Controllers;

namespace WordAccessManagementAddin.Controls
{
    public partial class ProtectUserControl : Window
    {
        private ProtectUserController _controller;

        public ProtectUserControl()
        {
            InitializeComponent();
            Loaded += HandleLoaded;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            var controller = new ProtectUserController();
            DataContext = controller.ViewModel;
        }
    }
}
