#region Usings

using System.ComponentModel;
using System.Windows.Input;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public partial class FreeThrowsSingleScoreWindow : ScoreWindowBase, IScoreWindow
    {
        public FreeThrowsSingleScoreWindow(WindowSettings settings,
                                           Player player,
                                           string gameTypeString,
                                           int legPoints) : base(settings)
        {
            InitializeComponent();
            PlayerAvatar.Source = player.Avatar;
            PlayerNameText.Text = $"{player.Name} {player.NickName}";
            GameTypeText.Text = gameTypeString;
            PointsText.Text = Converter.ToString(legPoints);
        }

        #region IScoreWindow

        public void SetSemaphore(DetectionServiceStatus status)
        {
            base.SetSemaphore(DetectionStatusSemaphore, status);
        }

        public void SetThrowNumber(ThrowNumber number)
        {
            base.SetThrowNumber(Throw1Rectangle,
                                Throw2Rectangle,
                                Throw3Rectangle,
                                number);
        }

        public void AddPointsTo(int pointsToAdd, Player player)
        {
            AddPoints(PointsText, pointsToAdd);
        }

        public new void Close()
        {
            base.Close();
        }

        #endregion

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}