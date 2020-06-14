#region Usings

using System.ComponentModel;
using System.Windows;

#endregion

namespace OneHundredAndEightyCore.Windows.CamsDetection
{
    public partial class CamsDetectionWindow
    {
        private readonly CamsDetectionBoard viewModel;

        public CamsDetectionWindow(CamsDetectionBoard viewModel)

        {
            this.viewModel = viewModel;
            InitializeComponent();

            DataContext = viewModel;
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

        private void UndoThrowButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.FireThrowUndo();
        }

        private void CorrectThrowButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.FireThrowCorrect();
        }
    }
}