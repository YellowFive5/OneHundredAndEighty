#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public partial class ClassicScoreWindow : ScoreWindowBase, IScoreWindow
    {
        private bool checkoutPlayer1Shown;
        private bool checkoutPlayer2Shown;
        private OnPlayer throwPointerOn;
        private OnPlayer onLegPointOn;

        public ClassicScoreWindow(WindowSettings windowSettings,
                                  List<Player> players,
                                  string gameType,
                                  int legPoints)
            : base(windowSettings)
        {
            InitializeComponent();

            var player1 = players[0];
            var player2 = players[1];

            Player1Avatar.Source = player1.Avatar;
            Player2Avatar.Source = player2.Avatar;
            Player1NameText.Text = $"{player1.Name} {player1.NickName}";
            Player2NameText.Text = $"{player2.Name} {player2.NickName}";
            GameTypeText.Text = gameType;
            PointsTextPlayer1.Text = Converter.ToString(legPoints);
            PointsTextPlayer2.Text = Converter.ToString(legPoints);
            CheckoutGridPlayer1.Opacity = 0;
            CheckoutGridPlayer2.Opacity = 0;
            throwPointerOn = OnPlayer.First;
            ThrowPointerGridPlayer2.Opacity = 0;
            onLegPointOn = OnPlayer.First;
            OnLegPointPlayer2.Opacity = 0;
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
            switch (player.Order)
            {
                case PlayerOrder.First:
                    AddPoints(PointsTextPlayer1, pointsToAdd);
                    break;
                case PlayerOrder.Second:
                    AddPoints(PointsTextPlayer2, pointsToAdd);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetPointsTo(Player player, int pointsToSet)
        {
            switch (player.Order)
            {
                case PlayerOrder.First:
                    SetPoints(PointsTextPlayer1, pointsToSet);
                    break;
                case PlayerOrder.Second:
                    SetPoints(PointsTextPlayer2, pointsToSet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void CheckoutShowOrUpdateFor(Player player, string hint)
        {
            switch (player.Order)
            {
                case PlayerOrder.First:
                    if (checkoutPlayer1Shown)
                    {
                        CheckoutUpdate(CheckoutTextPlayer1,
                                       hint);
                    }
                    else
                    {
                        CheckoutShow(CheckoutGridPlayer1,
                                     CheckoutTextPlayer1,
                                     hint);
                        checkoutPlayer1Shown = true;
                    }

                    break;
                case PlayerOrder.Second:
                    if (checkoutPlayer2Shown)
                    {
                        CheckoutUpdate(CheckoutTextPlayer2,
                                       hint);
                    }
                    else
                    {
                        CheckoutShow(CheckoutGridPlayer2,
                                     CheckoutTextPlayer2,
                                     hint);
                        checkoutPlayer2Shown = true;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void CheckoutHideFor(Player player)
        {
            switch (player.Order)
            {
                case PlayerOrder.First:
                    if (checkoutPlayer1Shown)
                    {
                        CheckoutHide(CheckoutGridPlayer1,
                                     CheckoutTextPlayer1);
                        checkoutPlayer1Shown = false;
                    }

                    break;
                case PlayerOrder.Second:
                    if (checkoutPlayer2Shown)
                    {
                        CheckoutHide(CheckoutGridPlayer2,
                                     CheckoutTextPlayer2);
                        checkoutPlayer2Shown = false;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ThrowPointerSetOn(Player player)
        {
            switch (player.Order)
            {
                case PlayerOrder.First:
                    if (throwPointerOn != OnPlayer.First)
                    {
                        FadeIn(ThrowPointerGridPlayer1);
                        FadeOut(ThrowPointerGridPlayer2);
                        throwPointerOn = OnPlayer.First;
                    }

                    break;
                case PlayerOrder.Second:
                    if (throwPointerOn != OnPlayer.Second)
                    {
                        FadeIn(ThrowPointerGridPlayer2);
                        FadeOut(ThrowPointerGridPlayer1);
                        throwPointerOn = OnPlayer.Second;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddLegsWonTo(Player player)
        {
            switch (player.Order)
            {
                case PlayerOrder.First:
                    AddPoints(LegsTextPlayer1, 1);
                    break;
                case PlayerOrder.Second:
                    AddPoints(LegsTextPlayer2, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetLegsWonTo(Player player, int legsToSet)
        {
            switch (player.Order)
            {
                case PlayerOrder.First:
                    SetPoints(LegsTextPlayer1, legsToSet);
                    break;
                case PlayerOrder.Second:
                    SetPoints(LegsTextPlayer2, legsToSet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddSetsWonTo(Player player)
        {
            switch (player.Order)
            {
                case PlayerOrder.First:
                    AddPoints(SetsTextPlayer1, 1);
                    break;
                case PlayerOrder.Second:
                    AddPoints(SetsTextPlayer2, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnLegPointSetOn(Player player)
        {
            switch (player.Order)
            {
                case PlayerOrder.First:
                    if (onLegPointOn != OnPlayer.First)
                    {
                        FadeIn(OnLegPointPlayer1);
                        FadeOut(OnLegPointPlayer2);
                        onLegPointOn = OnPlayer.First;
                    }

                    break;
                case PlayerOrder.Second:
                    if (onLegPointOn != OnPlayer.Second)
                    {
                        FadeIn(OnLegPointPlayer2);
                        FadeOut(OnLegPointPlayer1);
                        onLegPointOn = OnPlayer.Second;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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