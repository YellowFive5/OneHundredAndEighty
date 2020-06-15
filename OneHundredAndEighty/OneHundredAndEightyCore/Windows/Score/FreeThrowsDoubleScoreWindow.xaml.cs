#region Usings

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public partial class FreeThrowsDoubleScoreWindow
    {
        private readonly ScoreBoardService viewModel;

        public FreeThrowsDoubleScoreWindow(ScoreBoardService viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();

            DataContext = this.viewModel;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.OnWindowLoaded();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (!viewModel.ForceClose)
            {
                e.Cancel = true;
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}