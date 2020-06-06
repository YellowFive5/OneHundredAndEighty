#region Usings

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public partial class FreeThrowsDoubleScoreWindow : ScoreWindowBase, IScoreWindow
    {
        public FreeThrowsDoubleScoreWindow(WindowSettings windowSettings,
                                           List<Player> players,
                                           string gameType,
                                           int legPoints)
            : base(windowSettings)
        {
            InitializeComponent();
        }

        public void SetSemaphore(DetectionServiceStatus status)
        {
            throw new System.NotImplementedException();
        }

        public void SetThrowNumber(ThrowNumber number)
        {
            throw new System.NotImplementedException();
        }

        public void AddPointsTo(Player player, int pointsToAdd)
        {
            throw new System.NotImplementedException();
        }

        public void SetPointsTo(Player player, int pointsToSet)
        {
            throw new System.NotImplementedException();
        }

        public void CheckoutShowOrUpdateFor(Player player, string hint)
        {
            throw new System.NotImplementedException();
        }

        public void CheckoutHideFor(Player player)
        {
            throw new System.NotImplementedException();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}