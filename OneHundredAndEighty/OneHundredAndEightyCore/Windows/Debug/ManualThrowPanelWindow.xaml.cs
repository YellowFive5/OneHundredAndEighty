#region Usings

using System.Windows.Input;

#endregion

namespace OneHundredAndEightyCore.Windows.Debug
{
    public partial class ManualThrowPanelWindow
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