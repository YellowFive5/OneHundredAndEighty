#region Usings

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public partial class FreeThrowsDoubleScoreWindow : ScoreWindowBase
    {
        public FreeThrowsDoubleScoreWindow(ScoreBoardService viewModel)
            : base(viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}