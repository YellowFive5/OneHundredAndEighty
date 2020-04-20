#region Usings

using System.Windows;
using System.Windows.Input;

#endregion

namespace OneHundredAndEightyCore.Debug
{
    public partial class ManualThrowPanelWindow : Window
    {
        public ManualThrowPanelWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}