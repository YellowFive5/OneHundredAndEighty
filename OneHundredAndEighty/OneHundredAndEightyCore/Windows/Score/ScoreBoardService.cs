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
                    // scoreBoardWindow = new FreeThrowsDoubleScoreWindow();
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
            switch (scoreBoardType)
            {
                case ScoreBoardType.FreeThrowsSingle:
                    configService.Write(SettingsType.FreeThrowsSingleScoreWindowHeight, scoreBoardWindow.Height);
                    configService.Write(SettingsType.FreeThrowsSingleScoreWindowWidth, scoreBoardWindow.Width);
                    configService.Write(SettingsType.FreeThrowsSingleScoreWindowPositionLeft, scoreBoardWindow.Left);
                    configService.Write(SettingsType.FreeThrowsSingleScoreWindowPositionTop, scoreBoardWindow.Top);
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

        public void SetPointsTo(int pointsToSet, Player player)
        {
            throw new NotImplementedException();
        }

        public void AddPointsTo(int pointsToAdd, Player player)
        {
            scoreBoardWindow.AddPointsTo(pointsToAdd, player);
        }

        public void CheckPointsHintFor(Player player)
        {
            throw new NotImplementedException();
        }

        public void AddLegsWonTo(Player player)
        {
            throw new NotImplementedException();
        }

        public void SetLegsWonTo(int legsToSet, Player player)
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
            throw new NotImplementedException();
        }
    }
}