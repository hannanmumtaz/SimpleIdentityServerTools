using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private void HandleNumericField(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }
    }
}
