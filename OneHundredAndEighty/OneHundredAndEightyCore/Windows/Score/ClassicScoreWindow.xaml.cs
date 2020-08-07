#region Usings

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public partial class ClassicScoreWindow
    {
        public ClassicScoreWindow(ScoreBoardService viewModel)
            : base(viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}