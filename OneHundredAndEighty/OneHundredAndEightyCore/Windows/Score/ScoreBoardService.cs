#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public class ScoreBoardService : INotifyPropertyChanged
    {
        private readonly Logger logger;
        private readonly IConfigService configService;
        private readonly DrawService drawService;
        private Window scoreBoardWindow;
        private GameType gameType;

        public ScoreBoardService()
        {
        }

        public ScoreBoardService(Logger logger, IConfigService configService, DrawService drawService)
        {
            this.logger = logger;
            this.configService = configService;
            this.drawService = drawService;
        }

        #region Bindable props

        #region Window position

        private double windowPositionLeft;

        public double WindowPositionLeft
        {
            get => windowPositionLeft;
            set
            {
                windowPositionLeft = value;
                OnPropertyChanged(nameof(WindowPositionLeft));
            }
        }

        private double windowPositionTop;

        public double WindowPositionTop
        {
            get => windowPositionTop;
            set
            {
                windowPositionTop = value;
                OnPropertyChanged(nameof(WindowPositionTop));
            }
        }

        private double windowHeight;

        public double WindowHeight
        {
            get => windowHeight;
            set
            {
                windowHeight = value;
                OnPropertyChanged(nameof(WindowHeight));
            }
        }

        private double windowWidth;

        public double WindowWidth
        {
            get => windowWidth;
            set
            {
                windowWidth = value;
                OnPropertyChanged(nameof(WindowWidth));
            }
        }

        #endregion

        private BitmapImage player1Avatar;

        public BitmapImage Player1Avatar
        {
            get => player1Avatar;
            set
            {
                player1Avatar = value;
                OnPropertyChanged(nameof(Player1Avatar));
            }
        }

        private BitmapImage player2Avatar;

        public BitmapImage Player2Avatar
        {
            get => player2Avatar;
            set
            {
                player2Avatar = value;
                OnPropertyChanged(nameof(Player2Avatar));
            }
        }

        private SolidColorBrush detectionStatusLight;

        public SolidColorBrush DetectionStatusLight
        {
            get => detectionStatusLight;
            set
            {
                detectionStatusLight = value;
                OnPropertyChanged(nameof(DetectionStatusLight));
            }
        }

        private SolidColorBrush throw1Brush;

        public SolidColorBrush Throw1Brush
        {
            get => throw1Brush;
            set
            {
                throw1Brush = value;
                OnPropertyChanged(nameof(Throw1Brush));
            }
        }

        private SolidColorBrush throw2Brush;

        public SolidColorBrush Throw2Brush
        {
            get => throw2Brush;
            set
            {
                throw2Brush = value;
                OnPropertyChanged(nameof(Throw2Brush));
            }
        }

        private SolidColorBrush throw3Brush;

        public SolidColorBrush Throw3Brush
        {
            get => throw3Brush;
            set
            {
                throw3Brush = value;
                OnPropertyChanged(nameof(Throw3Brush));
            }
        }

        private string headerString;

        public string HeaderString
        {
            get => headerString;
            set
            {
                headerString = value;
                OnPropertyChanged(nameof(HeaderString));
            }
        }

        private string player1Name;

        public string Player1Name
        {
            get => player1Name;
            set
            {
                player1Name = value;
                OnPropertyChanged(nameof(Player1Name));
            }
        }

        private string player2Name;

        public string Player2Name
        {
            get => player2Name;
            set
            {
                player2Name = value;
                OnPropertyChanged(nameof(Player2Name));
            }
        }

        private int player1Points;

        public int Player1Points
        {
            get => player1Points;
            set
            {
                player1Points = value;
                OnPropertyChanged(nameof(Player1Points));
            }
        }

        private int player2Points;

        public int Player2Points
        {
            get => player2Points;
            set
            {
                player2Points = value;
                OnPropertyChanged(nameof(Player2Points));
            }
        }

        private int player1Legs;

        public int Player1Legs
        {
            get => player1Legs;
            set
            {
                player1Legs = value;
                OnPropertyChanged(nameof(Player1Legs));
            }
        }

        private int player2Legs;

        public int Player2Legs
        {
            get => player2Legs;
            set
            {
                player2Legs = value;
                OnPropertyChanged(nameof(Player2Legs));
            }
        }

        private int player1Sets;

        public int Player1Sets
        {
            get => player1Sets;
            set
            {
                player1Sets = value;
                OnPropertyChanged(nameof(Player1Sets));
            }
        }

        private int player2Sets;

        public int Player2Sets
        {
            get => player2Sets;
            set
            {
                player2Sets = value;
                OnPropertyChanged(nameof(Player2Sets));
            }
        }

        private string player1Hint;

        public string Player1Hint
        {
            get => player1Hint;
            set
            {
                player1Hint = value;
                OnPropertyChanged(nameof(Player1Hint));
            }
        }

        private bool player1HintShown;

        public bool Player1HintShown
        {
            get => player1HintShown;
            set
            {
                player1HintShown = value;
                OnPropertyChanged(nameof(Player1HintShown));
            }
        }

        private string player2Hint;

        public string Player2Hint
        {
            get => player2Hint;
            set
            {
                player2Hint = value;
                OnPropertyChanged(nameof(Player2Hint));
            }
        }

        private bool player2HintShown;

        public bool Player2HintShown
        {
            get => player2HintShown;
            set
            {
                player2HintShown = value;
                OnPropertyChanged(nameof(Player2HintShown));
            }
        }

        private bool player1OnLegPointShown;

        public bool Player1OnLegPointShown
        {
            get => player1OnLegPointShown;
            set
            {
                player1OnLegPointShown = value;
                OnPropertyChanged(nameof(Player1OnLegPointShown));
            }
        }

        private bool player2OnLegPointShown;

        public bool Player2OnLegPointShown
        {
            get => player2OnLegPointShown;
            set
            {
                player2OnLegPointShown = value;
                OnPropertyChanged(nameof(Player2OnLegPointShown));
            }
        }

        private bool player1OnThrowPointerShown;

        public bool Player1OnThrowPointerShown
        {
            get => player1OnThrowPointerShown;
            set
            {
                player1OnThrowPointerShown = value;
                OnPropertyChanged(nameof(Player1OnThrowPointerShown));
            }
        }

        private bool player2OnThrowPointerShown;

        public bool Player2OnThrowPointerShown
        {
            get => player2OnThrowPointerShown;
            set
            {
                player2OnThrowPointerShown = value;
                OnPropertyChanged(nameof(Player2OnThrowPointerShown));
            }
        }

        #endregion

        #region Open/Close

        public bool ForceClose { get; private set; }

        public void OpenScoreBoard(GameType gameType,
                                   List<Player> players,
                                   string gameTypeString,
                                   int legPoints = 0)
        {
            var player1 = players.ElementAt(0);

            Player player2 = null; //  todo ugly
            if (players.Count > 1)
            {
                player2 = players.ElementAt(1);
            }

            this.gameType = gameType;
            switch (gameType)
            {
                case GameType.FreeThrowsSingle:
                    scoreBoardWindow = new FreeThrowsSingleScoreWindow(this);
                    break;
                case GameType.FreeThrowsDouble:
                    scoreBoardWindow = new FreeThrowsDoubleScoreWindow(this);
                    Player2Avatar = player2.Avatar;
                    Player2Points = legPoints;
                    Player2Name = $"{player2.Name} {player2.NickName}";
                    Player1OnLegPointShown = true;
                    Player1OnThrowPointerShown = true;
                    break;
                case GameType.Classic:
                    scoreBoardWindow = new ClassicScoreWindow(this);
                    Player2Avatar = player2.Avatar;
                    Player2Points = legPoints;
                    Player2Name = $"{player2.Name} {player2.NickName}";
                    Player1OnLegPointShown = true;
                    Player1OnThrowPointerShown = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Player1Sets = 0;
            Player2Sets = 0;
            Player1Legs = 0;
            Player2Legs = 0;

            Player1Hint = string.Empty;
            Player2Hint = string.Empty;
            Player1HintShown = false;
            Player2HintShown = false;

            HeaderString = gameTypeString;
            SetDetectionStatus(DetectionServiceStatus.WaitingThrow);
            SetThrowNumber(ThrowNumber.FirstThrow);
            Player1Avatar = player1.Avatar;
            Player1Points = legPoints;
            Player1Name = $"{player1.Name} {player1.NickName}";

            scoreBoardWindow.Show();
        }

        public void OnWindowLoaded()
        {
            switch (gameType)
            {
                case GameType.FreeThrowsSingle:
                    WindowHeight = configService.FreeThrowsSingleScoreWindowHeight;
                    WindowWidth = configService.FreeThrowsSingleScoreWindowWidth;
                    WindowPositionLeft = configService.FreeThrowsSingleScoreWindowPositionLeft;
                    WindowPositionTop = configService.FreeThrowsSingleScoreWindowPositionTop;
                    break;
                case GameType.FreeThrowsDouble:
                    WindowHeight = configService.FreeThrowsDoubleScoreWindowHeight;
                    WindowWidth = configService.FreeThrowsDoubleScoreWindowWidth;
                    WindowPositionLeft = configService.FreeThrowsDoubleScoreWindowPositionLeft;
                    WindowPositionTop = configService.FreeThrowsDoubleScoreWindowPositionTop;
                    break;
                case GameType.Classic:
                    WindowHeight = configService.ClassicScoreWindowHeight;
                    WindowWidth = configService.ClassicScoreWindowWidth;
                    WindowPositionLeft = configService.ClassicScoreWindowPositionLeft;
                    WindowPositionTop = configService.ClassicScoreWindowPositionTop;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void CloseScoreBoard()
        {
            if (scoreBoardWindow == null)
            {
                return;
            }

            switch (gameType)
            {
                case GameType.FreeThrowsSingle:
                    configService.FreeThrowsSingleScoreWindowHeight = WindowHeight;
                    configService.FreeThrowsSingleScoreWindowWidth = WindowWidth;
                    configService.FreeThrowsSingleScoreWindowPositionLeft = WindowPositionLeft;
                    configService.FreeThrowsSingleScoreWindowPositionTop = WindowPositionTop;
                    break;
                case GameType.FreeThrowsDouble:
                    configService.FreeThrowsDoubleScoreWindowHeight = WindowHeight;
                    configService.FreeThrowsDoubleScoreWindowWidth = WindowWidth;
                    configService.FreeThrowsDoubleScoreWindowPositionLeft = WindowPositionLeft;
                    configService.FreeThrowsDoubleScoreWindowPositionTop = WindowPositionTop;
                    break;
                case GameType.Classic:
                    configService.ClassicScoreWindowHeight = WindowHeight;
                    configService.ClassicScoreWindowWidth = WindowWidth;
                    configService.ClassicScoreWindowPositionLeft = WindowPositionLeft;
                    configService.ClassicScoreWindowPositionTop = WindowPositionTop;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ForceClose = true;
            scoreBoardWindow?.Close();
            scoreBoardWindow = null;
            ForceClose = false;
        }

        #endregion

        public void SetDetectionStatus(DetectionServiceStatus status)
        {
            switch (status)
            {
                case DetectionServiceStatus.WaitingThrow:
                    DetectionStatusLight = drawService.greenBrush;
                    break;
                case DetectionServiceStatus.ProcessingThrow:
                    DetectionStatusLight = drawService.redBrush;
                    break;
                case DetectionServiceStatus.DartsExtraction:
                    DetectionStatusLight = drawService.yellowBrush;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        public void SetThrowNumber(ThrowNumber throwNumber)
        {
            switch (throwNumber)
            {
                case ThrowNumber.FirstThrow:
                    Throw1Brush = drawService.blackBrush;
                    Throw2Brush = drawService.blackBrush;
                    Throw3Brush = drawService.blackBrush;
                    break;
                case ThrowNumber.SecondThrow:
                    Throw1Brush = drawService.transparentBrush;
                    Throw2Brush = drawService.blackBrush;
                    Throw3Brush = drawService.blackBrush;
                    break;
                case ThrowNumber.ThirdThrow:
                    Throw1Brush = drawService.transparentBrush;
                    Throw2Brush = drawService.transparentBrush;
                    Throw3Brush = drawService.blackBrush;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(throwNumber), throwNumber, null);
            }
        }

        public void SetPointsTo(Player player, int pointsToSet)
        {
            switch (player.Order)
            {
                case PlayerOrder.First:
                    Player1Points = pointsToSet;
                    break;
                case PlayerOrder.Second:
                    Player2Points = pointsToSet;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddPointsTo(Player player, int pointsToAdd)
        {
            switch (player.Order)
            {
                case PlayerOrder.First:
                    Player1Points = Player1Points + pointsToAdd;
                    break;
                case PlayerOrder.Second:
                    Player2Points = Player2Points + pointsToAdd;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void CheckPointsHintFor(Player player)
        {
            var hint = CheckOut.Get(player.LegPoints, player.ThrowNumber);
            if (hint != null)
            {
                switch (player.Order)
                {
                    case PlayerOrder.First:
                        Player1Hint = hint;
                        Player1HintShown = true;
                        break;
                    case PlayerOrder.Second:
                        Player2Hint = hint;
                        Player2HintShown = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                switch (player.Order)
                {
                    case PlayerOrder.First:
                        Player1Hint = string.Empty;
                        Player1HintShown = false;
                        break;
                    case PlayerOrder.Second:
                        Player2Hint = string.Empty;
                        Player2HintShown = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void AddLegsWonTo(Player player)
        {
            switch (player.Order)
            {
                case PlayerOrder.First:
                    Player1Legs = Player1Legs + 1;
                    break;
                case PlayerOrder.Second:
                    Player2Legs = Player2Legs + 1;
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
                    Player1Legs = legsToSet;
                    break;
                case PlayerOrder.Second:
                    Player2Legs = legsToSet;
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
                    Player1Sets = Player1Sets + 1;
                    break;
                case PlayerOrder.Second:
                    Player2Sets = Player2Sets + 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnThrowPointerSetOn(Player player)
        {
            switch (player.Order)
            {
                case PlayerOrder.First:
                    Player1OnThrowPointerShown = true;
                    Player2OnThrowPointerShown = false;
                    break;
                case PlayerOrder.Second:
                    Player2OnThrowPointerShown = true;
                    Player1OnThrowPointerShown = false;
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
                    Player1OnLegPointShown = true;
                    Player2OnLegPointShown = false;
                    break;
                case PlayerOrder.Second:
                    Player2OnLegPointShown = true;
                    Player1OnLegPointShown = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region PropertyChangingFire

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}