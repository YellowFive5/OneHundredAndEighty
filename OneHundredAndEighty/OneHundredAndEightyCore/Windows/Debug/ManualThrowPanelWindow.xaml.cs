#region Usings

using System.Windows;
using System.Windows.Input;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Windows.DebugPanel
{
    public partial class ManualThrowPanelWindow
    {
        private readonly ManualThrowPanel windowViewModel;

        public ManualThrowPanelWindow(ManualThrowPanel windowViewModel)
        {
            this.windowViewModel = windowViewModel;
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var multiplier = (e.Source as FrameworkElement)?.Name;
            var sector = Converter.ToInt((e.Source as FrameworkElement)?.Tag.ToString());

            windowViewModel.ThrowDartTo(multiplier, sector);
        }
    }
}