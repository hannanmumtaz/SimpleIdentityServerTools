using System.Windows;
using WordAccessManagementAddin.Controls.Controllers;

namespace WordAccessManagementAddin.Controls
{
    public partial class ProtectUserControl : Window
    {
        public ProtectUserControl()
        {
            InitializeComponent();
            var controller = new ProtectUserController(this);
            DataContext = controller.ViewModel;
        }
    }
}
