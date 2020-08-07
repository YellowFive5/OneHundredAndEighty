#region Usings

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public partial class FreeThrowsSingleScoreWindow : ScoreWindowBase
    {
        public FreeThrowsSingleScoreWindow(ScoreBoardService viewModel)
            : base(viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}