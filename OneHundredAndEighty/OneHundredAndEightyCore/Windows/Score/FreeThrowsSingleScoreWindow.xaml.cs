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
        protected bool CheckoutShown;

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
            CheckoutGrid.Opacity = 0;
        }

        #region IScoreWindow

        public new void Close()
        {
            base.Close();
        }

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

        public void AddPointsTo(Player player, int pointsToAdd)
        {
            AddPoints(PointsText, pointsToAdd);
        }

        public void SetPointsTo(Player player, int pointsToSet)
        {
            SetPoints(PointsText, pointsToSet);
        }

        public void CheckoutShowOrUpdateFor(Player player, string hint)
        {
            if (CheckoutShown)
            {
                CheckoutUpdate(CheckoutText,
                               hint);
            }
            else
            {
                CheckoutShow(CheckoutGrid,
                             CheckoutText,
                             hint);
                CheckoutShown = true;
            }
        }

        public void CheckoutHideFor(Player player)
        {
            if (CheckoutShown)
            {
                CheckoutHide(CheckoutGrid,
                             CheckoutText);
                CheckoutShown = false;
            }
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