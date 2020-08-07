#region Usings

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public class ScoreWindowBase : Window
    {
        private readonly ScoreBoardService viewModel;

        protected ScoreWindowBase(ScoreBoardService viewModel)
        {
            this.viewModel = viewModel;
        }

        protected void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.OnWindowLoaded();
        }

        protected void OnClosing(object sender, CancelEventArgs e)
        {
            if (!viewModel.ForceClose)
            {
                e.Cancel = true;
            }
        }

        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        protected void UndoThrowButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.FireThrowUndo();
        }

        protected void CorrectThrowButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.FireThrowCorrect();
        }
    }
}