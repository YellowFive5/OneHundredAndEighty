#region Usings

using System.Windows.Controls;
using System.Windows.Input;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Windows.Main.Tabs.Settings
{
    public partial class SettingsTabView : UserControl
    {
        public SettingsTabView()
        {
            InitializeComponent();
        }

        private void IntValidation(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Validator.ValidateIntInput(e.Text);
        }

        private void DoubleValidation(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Validator.ValidateDoubleInput(e.Text);
        }
    }
}