#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public class ScoreBoardService
    {
        private readonly Logger logger;
        private readonly ConfigService configService;
        private IScoreWindow scoreBoardWindow;
        private ScoreBoardType scoreBoardType;

        public ScoreBoardService(Logger logger, ConfigService configService)
        {
            this.logger = logger;
            this.configService = configService;
        }

        #region Open/Close

        public void OpenScoreBoard(GameType type,
                                   List<Player> players,
                                   string gameTypeString,
                                   int legPoints = 0)
        {
            var windowSettings = LoadSettings();

            switch (type)
            {
                case GameType.FreeThrowsSingle:
                    scoreBoardType = ScoreBoardType.FreeThrowsSingle;
                    scoreBoardWindow = new FreeThrowsSingleScoreWindow(windowSettings,
                                                                       players.First(),
                                                                       gameTypeString,
                                                                       legPoints);
                    break;
                case GameType.FreeThrowsDouble:
                    scoreBoardType = ScoreBoardType.FreeThrowsDouble;
                    scoreBoardWindow = new FreeThrowsDoubleScoreWindow(windowSettings,
                                                                       players,
                                                                       gameTypeString,
                                                                       legPoints);
                    break;
                case GameType.Classic:
                    scoreBoardType = ScoreBoardType.Classic;
                    // scoreBoardWindow = new ClassicScoreWindow();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            scoreBoardWindow.Show();
        }

        public void CloseScoreBoard()
        {
            if (scoreBoardWindow != null)
            {
                SaveSettings();
                scoreBoardWindow?.Close();
            }
        }

        private void SaveSettings()
        {
            var windowSettings = scoreBoardWindow.GetWindowSettings();
            switch (scoreBoardType)
            {
                case ScoreBoardType.FreeThrowsSingle:
                    configService.Write(SettingsType.FreeThrowsSingleScoreWindowHeight, windowSettings.Height);
                    configService.Write(SettingsType.FreeThrowsSingleScoreWindowWidth, windowSettings.Width);
                    configService.Write(SettingsType.FreeThrowsSingleScoreWindowPositionLeft, windowSettings.PositionLeft);
                    configService.Write(SettingsType.FreeThrowsSingleScoreWindowPositionTop, windowSettings.PositionTop);
                    break;
                case ScoreBoardType.FreeThrowsDouble:
                    throw new ArgumentOutOfRangeException();
                case ScoreBoardType.Classic:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private WindowSettings LoadSettings()
        {
            switch (scoreBoardType)
            {
                case ScoreBoardType.FreeThrowsSingle:
                    return new WindowSettings(configService.Read<double>(SettingsType.FreeThrowsSingleScoreWindowHeight),
                                              configService.Read<double>(SettingsType.FreeThrowsSingleScoreWindowWidth),
                                              configService.Read<double>(SettingsType.FreeThrowsSingleScoreWindowPositionLeft),
                                              configService.Read<double>(SettingsType.FreeThrowsSingleScoreWindowPositionTop));
                case ScoreBoardType.FreeThrowsDouble:
                    throw new ArgumentOutOfRangeException();
                case ScoreBoardType.Classic:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        public void SetPointsTo(Player player, int pointsToSet)
        {
            scoreBoardWindow.SetPointsTo(player, pointsToSet);
        }

        public void AddPointsTo(Player player, int pointsToAdd)
        {
            scoreBoardWindow.AddPointsTo(player, pointsToAdd);
        }

        public void CheckPointsHintFor(Player player)
        {
            var hint = CheckOut.Get(player.LegPoints, player.ThrowNumber);
            if (hint != null)
            {
                scoreBoardWindow.CheckoutShowOrUpdateFor(player, hint);
            }
            else
            {
                scoreBoardWindow.CheckoutHideFor(player);
            }
        }

        public void AddLegsWonTo(Player player)
        {
            throw new NotImplementedException();
        }

        public void SetLegsWonTo(Player player, int legsToSet)
        {
            throw new NotImplementedException();
        }

        public void AddSetsWonTo(Player player)
        {
            throw new NotImplementedException();
        }

        public void SetDetectionStatus(DetectionServiceStatus status)
        {
            scoreBoardWindow.SetSemaphore(status);
        }

        public void SetThrowNumber(ThrowNumber throwNumber)
        {
            scoreBoardWindow.SetThrowNumber(throwNumber);
        }

        public void LegPointSetOn(Player player)
        {
            throw new NotImplementedException();
        }

        public void OnThrowPointerSetOn(Player player)
        {
        }
    }
}