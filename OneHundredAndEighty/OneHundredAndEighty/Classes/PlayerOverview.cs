using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OneHundredAndEighty
{
    public static class PlayerOverview  //  Логика показа вкладки данных игроков
    {
        static MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Cсылка на главное окно
        public static void ClearPanel() //  Зануляем данные вкладки
        {
            ClearPlayer1();
            ClearPlayer2();
            ClearPvP();
            MainWindow.Player2TabNameCombobox.SelectedIndex = -1;
            MainWindow.Player1TabNameCombobox.SelectedIndex = -1;
        }
        public static void ClearPvP()   //  Зануляем окно PvP
        {
            foreach (FrameworkElement item in MainWindow.PvPPanel.Children)
            {
                if (item.Tag != null)
                {
                    if (item.GetType() == typeof(Label))
                    {
                        Label L = item as Label;
                        L.Content = 0;
                    }
                    if (item.GetType() == typeof(Rectangle))
                    {
                        Rectangle R = item as Rectangle;
                        R.Fill = Brush(0, 0);
                    }
                }
            }
        }
        public static void ClearPlayer1()   //  Зануляем данные игрока 1
        {
            MainWindow.TabPlayer1GamesPlayed.Content = 0;
            MainWindow.TabPlayer1GamesWon.Content = 0;
            MainWindow.TabPlayer1SetsPlayed.Content = 0;
            MainWindow.TabPlayer1SetsWon.Content = 0;
            MainWindow.TabPlayer1LegsPlayed.Content = 0;
            MainWindow.TabPlayer1LegsWon.Content = 0;
            MainWindow.TabPlayer1Throws.Content = 0;
            MainWindow.TabPlayer1Points.Content = 0;
            MainWindow.TabPlayer1AvaragePoints.Content = 0;
            MainWindow.TabPlayer1AvarageHandPoints.Content = 0;
            MainWindow.TabPlayer1BestHand.Content = 0;
            MainWindow.TabPlayer1_180.Content = 0;
            MainWindow.TabPlayer1FaultThrows.Content = 0;
            MainWindow.TabPlayer1TrembleThrow.Content = 0;
            MainWindow.TabPlayer1BulleyeThrow.Content = 0;
            MainWindow.TabPlayer1DoubleThrow.Content = 0;
            MainWindow.TabPlayer1_25Throw.Content = 0;
            MainWindow.TabPlayer1SingleThrow.Content = 0;
            MainWindow.TabPlayer1ZeroThrow.Content = 0;
            DrawSliders();
            MainWindow.AchievePlayer110MatchesLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer1100MatchesLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer11000MatchesLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer110WinLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer1100WinLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer11000WinLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer11000ThrowsLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer110000ThrowsLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer1100000ThrowsLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer110000PointsLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer1100000PointsLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer11000000PointsLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer1180x10Light.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer1180x100Light.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer1180x1000Light.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer1First180Light.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer13BullLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer1mrZLight.Visibility = System.Windows.Visibility.Hidden;
        }
        public static void ClearPlayer2()   //  Зануляем данные игрока 2
        {
            MainWindow.TabPlayer2GamesPlayed.Content = 0;
            MainWindow.TabPlayer2GamesWon.Content = 0;
            MainWindow.TabPlayer2SetsPlayed.Content = 0;
            MainWindow.TabPlayer2SetsWon.Content = 0;
            MainWindow.TabPlayer2LegsPlayed.Content = 0;
            MainWindow.TabPlayer2LegsWon.Content = 0;
            MainWindow.TabPlayer2Throws.Content = 0;
            MainWindow.TabPlayer2Points.Content = 0;
            MainWindow.TabPlayer2AvaragePoints.Content = 0;
            MainWindow.TabPlayer2AvarageHandPoints.Content = 0;
            MainWindow.TabPlayer2BestHand.Content = 0;
            MainWindow.TabPlayer2_180.Content = 0;
            MainWindow.TabPlayer2FaultThrows.Content = 0;
            MainWindow.TabPlayer2TrembleThrow.Content = 0;
            MainWindow.TabPlayer2BulleyeThrow.Content = 0;
            MainWindow.TabPlayer2DoubleThrow.Content = 0;
            MainWindow.TabPlayer2_25Throw.Content = 0;
            MainWindow.TabPlayer2SingleThrow.Content = 0;
            MainWindow.TabPlayer2ZeroThrow.Content = 0;
            DrawSliders();
            MainWindow.AchievePlayer210MatchesLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer2100MatchesLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer21000MatchesLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer210WinLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer2100WinLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer21000WinLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer21000ThrowsLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer210000ThrowsLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer2100000ThrowsLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer210000PointsLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer2100000PointsLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer21000000PointsLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer2180x10Light.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer2180x100Light.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer2180x1000Light.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer2First180Light.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer23BullLight.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.AchievePlayer2mrZLight.Visibility = System.Windows.Visibility.Hidden;

        }
        public static void RefreshPlayer1(int PlayerId) //  Обновить данные игрока 1
        {
            DataTable PlayerData = DBwork.LoadPlayerData(PlayerId);
            MainWindow.TabPlayer1GamesPlayed.Content = (int)(PlayerData.Rows[0][4]);
            MainWindow.TabPlayer1GamesWon.Content = (int)(PlayerData.Rows[0][5]);
            MainWindow.TabPlayer1SetsPlayed.Content = (int)(PlayerData.Rows[0][7]);
            MainWindow.TabPlayer1SetsWon.Content = (int)(PlayerData.Rows[0][8]);
            MainWindow.TabPlayer1LegsPlayed.Content = (int)(PlayerData.Rows[0][9]);
            MainWindow.TabPlayer1LegsWon.Content = (int)(PlayerData.Rows[0][10]);
            MainWindow.TabPlayer1Throws.Content = (int)(PlayerData.Rows[0][11]);
            MainWindow.TabPlayer1Points.Content = (int)(PlayerData.Rows[0][12]);
            MainWindow.TabPlayer1AvaragePoints.Content = Math.Round((double)(PlayerData.Rows[0][13]), 2);
            MainWindow.TabPlayer1AvarageHandPoints.Content = Math.Round((double)(PlayerData.Rows[0][14]), 2);
            MainWindow.TabPlayer1BestHand.Content = Math.Round((double)(PlayerData.Rows[0][15]), 2);
            MainWindow.TabPlayer1_180.Content = (int)(PlayerData.Rows[0][16]);
            MainWindow.TabPlayer1TrembleThrow.Content = (int)(PlayerData.Rows[0][17]);
            MainWindow.TabPlayer1BulleyeThrow.Content = (int)(PlayerData.Rows[0][18]);
            MainWindow.TabPlayer1DoubleThrow.Content = (int)(PlayerData.Rows[0][19]);
            MainWindow.TabPlayer1SingleThrow.Content = (int)(PlayerData.Rows[0][20]);
            MainWindow.TabPlayer1_25Throw.Content = (int)(PlayerData.Rows[0][21]);
            MainWindow.TabPlayer1ZeroThrow.Content = (int)(PlayerData.Rows[0][22]);
            MainWindow.TabPlayer1FaultThrows.Content = (int)(PlayerData.Rows[0][23]);
            DrawSliders();
            if ((bool)(PlayerData.Rows[0][24]))
                MainWindow.AchievePlayer110MatchesLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][25]))
                MainWindow.AchievePlayer1100MatchesLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][26]))
                MainWindow.AchievePlayer11000MatchesLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][27]))
                MainWindow.AchievePlayer110WinLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][28]))
                MainWindow.AchievePlayer1100WinLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][29]))
                MainWindow.AchievePlayer11000WinLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][30]))
                MainWindow.AchievePlayer11000ThrowsLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][31]))
                MainWindow.AchievePlayer110000ThrowsLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][32]))
                MainWindow.AchievePlayer1100000ThrowsLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][33]))
                MainWindow.AchievePlayer110000PointsLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][34]))
                MainWindow.AchievePlayer1100000PointsLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][35]))
                MainWindow.AchievePlayer11000000PointsLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][36]))
                MainWindow.AchievePlayer1180x10Light.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][37]))
                MainWindow.AchievePlayer1180x100Light.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][38]))
                MainWindow.AchievePlayer1180x1000Light.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][39]))
                MainWindow.AchievePlayer1First180Light.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][40]))
                MainWindow.AchievePlayer13BullLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][41]))
                MainWindow.AchievePlayer1mrZLight.Visibility = System.Windows.Visibility.Visible;
        }
        public static void RefreshPlayer2(int PlayerId) //  Обновить данные игрока 2
        {
            DataTable PlayerData = DBwork.LoadPlayerData(PlayerId);
            MainWindow.TabPlayer2GamesPlayed.Content = (int)(PlayerData.Rows[0][4]);
            MainWindow.TabPlayer2GamesWon.Content = (int)(PlayerData.Rows[0][5]);
            MainWindow.TabPlayer2SetsPlayed.Content = (int)(PlayerData.Rows[0][7]);
            MainWindow.TabPlayer2SetsWon.Content = (int)(PlayerData.Rows[0][8]);
            MainWindow.TabPlayer2LegsPlayed.Content = (int)(PlayerData.Rows[0][9]);
            MainWindow.TabPlayer2LegsWon.Content = (int)(PlayerData.Rows[0][10]);
            MainWindow.TabPlayer2Throws.Content = (int)(PlayerData.Rows[0][11]);
            MainWindow.TabPlayer2Points.Content = (int)(PlayerData.Rows[0][12]);
            MainWindow.TabPlayer2AvaragePoints.Content = Math.Round((double)(PlayerData.Rows[0][13]), 2);
            MainWindow.TabPlayer2AvarageHandPoints.Content = Math.Round((double)(PlayerData.Rows[0][14]), 2);
            MainWindow.TabPlayer2BestHand.Content = Math.Round((double)(PlayerData.Rows[0][15]), 2);
            MainWindow.TabPlayer2_180.Content = (int)(PlayerData.Rows[0][16]);
            MainWindow.TabPlayer2TrembleThrow.Content = (int)(PlayerData.Rows[0][17]);
            MainWindow.TabPlayer2BulleyeThrow.Content = (int)(PlayerData.Rows[0][18]);
            MainWindow.TabPlayer2DoubleThrow.Content = (int)(PlayerData.Rows[0][19]);
            MainWindow.TabPlayer2SingleThrow.Content = (int)(PlayerData.Rows[0][20]);
            MainWindow.TabPlayer2_25Throw.Content = (int)(PlayerData.Rows[0][21]);
            MainWindow.TabPlayer2ZeroThrow.Content = (int)(PlayerData.Rows[0][22]);
            MainWindow.TabPlayer2FaultThrows.Content = (int)(PlayerData.Rows[0][23]);
            DrawSliders();
            if ((bool)(PlayerData.Rows[0][24]))
                MainWindow.AchievePlayer210MatchesLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][25]))
                MainWindow.AchievePlayer2100MatchesLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][26]))
                MainWindow.AchievePlayer21000MatchesLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][27]))
                MainWindow.AchievePlayer210WinLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][28]))
                MainWindow.AchievePlayer2100WinLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][29]))
                MainWindow.AchievePlayer21000WinLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][30]))
                MainWindow.AchievePlayer21000ThrowsLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][31]))
                MainWindow.AchievePlayer210000ThrowsLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][32]))
                MainWindow.AchievePlayer2100000ThrowsLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][33]))
                MainWindow.AchievePlayer210000PointsLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][34]))
                MainWindow.AchievePlayer2100000PointsLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][35]))
                MainWindow.AchievePlayer21000000PointsLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][36]))
                MainWindow.AchievePlayer2180x10Light.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][37]))
                MainWindow.AchievePlayer2180x100Light.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][38]))
                MainWindow.AchievePlayer2180x1000Light.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][39]))
                MainWindow.AchievePlayer2First180Light.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][40]))
                MainWindow.AchievePlayer23BullLight.Visibility = System.Windows.Visibility.Visible;
            if ((bool)(PlayerData.Rows[0][41]))
                MainWindow.AchievePlayer2mrZLight.Visibility = System.Windows.Visibility.Visible;
        }
        public static void RefreshPvP(int Player1Id, int Player2Id)
        {
            int[] arr = new int[9];
            arr = DBwork.FindPvP(Player1Id, Player2Id);
            MainWindow.TabPvPGames.Content = arr[0];
            MainWindow.TabPlayer1PvPGames.Content = arr[1];
            MainWindow.TabPlayer2PvPGames.Content = arr[2];
            MainWindow.TabPvPGamesSlider.Fill = Brush(arr[1], arr[2]);

            MainWindow.TabPvPLegs.Content = arr[3];
            MainWindow.TabPlayer1PvPLegs.Content = arr[4];
            MainWindow.TabPlayer2PvPLegs.Content = arr[5];
            MainWindow.TabPvPLegsSlider.Fill = Brush(arr[4], arr[5]);

            MainWindow.TabPvPSets.Content = arr[6];
            MainWindow.TabPlayer1PvPSets.Content = arr[7];
            MainWindow.TabPlayer2PvPSets.Content = arr[8];
            MainWindow.TabPvPSetsSlider.Fill = Brush(arr[7], arr[8]);

            MainWindow.TabPvPNumberOfThrows.Content = arr[9];
            MainWindow.TabPvPPlayer1Throws.Content = arr[10];
            MainWindow.TabPvPPlayer2Throws.Content = arr[11];
            MainWindow.TabPvPPlayersThrowsSlider.Fill = Brush(arr[10], arr[11]);

            MainWindow.TabPvPNumberOfPoints.Content = arr[12];
            MainWindow.TabPvPPlayer1Points.Content = arr[13];
            MainWindow.TabPvPPlayer2Points.Content = arr[14];
            MainWindow.TabPvPPlayersPointsSlider.Fill = Brush(arr[13], arr[14]);

            MainWindow.TabPvP180.Content = arr[15];
            MainWindow.TabPlayer1PvP180.Content = arr[16];
            MainWindow.TabPlayer2PvP180.Content = arr[17];
            MainWindow.TabPvP180Slider.Fill = Brush(arr[16], arr[17]);

            MainWindow.TabPvPTrembleThrows.Content = arr[18];
            MainWindow.TabPlayer1PvPTrembleThrows.Content = arr[19];
            MainWindow.TabPlayer2PvPTrembleThrows.Content = arr[20];
            MainWindow.TabPvPTrembleThrowsSlider.Fill = Brush(arr[19], arr[20]);

            MainWindow.TabPvPBulleyethrows.Content = arr[21];
            MainWindow.TabPlayer1PvPBullEyeThrows.Content = arr[22];
            MainWindow.TabPlayer2PvPBullEyeThrows.Content = arr[23];
            MainWindow.TabPvPBulleyeThrowsSlider.Fill = Brush(arr[22], arr[23]);

            MainWindow.TabPvPDoubleThrows.Content = arr[24];
            MainWindow.TabPvPPlayer1DoubleThrows.Content = arr[25];
            MainWindow.TabPvPPlayer2DoubleThrows.Content = arr[26];
            MainWindow.TabPvPDoubleThrowsSlider.Fill = Brush(arr[25], arr[26]);

            MainWindow.TabPvP25Throws.Content = arr[27];
            MainWindow.TabPlayer1PvP25Throws.Content = arr[28];
            MainWindow.TabPlayer2PvP25Throws.Content = arr[29];
            MainWindow.TabPvP25ThrowsSlider.Fill = Brush(arr[28], arr[29]);

            MainWindow.TabPvPSingleThrows.Content = arr[30];
            MainWindow.TabPlayer1PvPSingleThrows.Content = arr[31];
            MainWindow.TabPlayer2PvPSingleThrows.Content = arr[32];
            MainWindow.TabPvPSingleThrowsSlider.Fill = Brush(arr[31], arr[32]);

            MainWindow.TabPvPZeroThrows.Content = arr[33];
            MainWindow.TabPlayer1PvPZeroThrows.Content = arr[34];
            MainWindow.TabPlayer2PvPZeroThrows.Content = arr[35];
            MainWindow.TabPvPZeroThrowsSlider.Fill = Brush(arr[34], arr[35]);

            MainWindow.TabPvPFaultThrows.Content = arr[36];
            MainWindow.TabPlayer1PvPFaultThrows.Content = arr[37];
            MainWindow.TabPlayer2PvPFaultThrows.Content = arr[38];
            MainWindow.TabPvPFaultThrowsSlider.Fill = Brush(arr[37], arr[38]);
        }
        static void DrawSliders()   //  Отрисовка слайдеров
        {
            MainWindow.TabPlayersGamesPlayedSlider.Fill = Brush(MainWindow.TabPlayer1GamesPlayed.Content, MainWindow.TabPlayer2GamesPlayed.Content);
            MainWindow.TabPlayersGamesWonSlider.Fill = Brush(MainWindow.TabPlayer1GamesWon.Content, MainWindow.TabPlayer2GamesWon.Content);
            MainWindow.TabPlayersSetsPlayedSlider.Fill = Brush(MainWindow.TabPlayer1SetsPlayed.Content, MainWindow.TabPlayer2SetsPlayed.Content);
            MainWindow.TabPlayersSetsWonSlider.Fill = Brush(MainWindow.TabPlayer1SetsWon.Content, MainWindow.TabPlayer2SetsWon.Content);
            MainWindow.TabPlayersLegsPlayedSlider.Fill = Brush(MainWindow.TabPlayer1LegsPlayed.Content, MainWindow.TabPlayer2LegsPlayed.Content);
            MainWindow.TabPlayersLegsWonSlider.Fill = Brush(MainWindow.TabPlayer1LegsWon.Content, MainWindow.TabPlayer2LegsWon.Content);
            MainWindow.TabPlayersThrowsSlider.Fill = Brush(MainWindow.TabPlayer1Throws.Content, MainWindow.TabPlayer2Throws.Content);
            MainWindow.TabPlayersPointsSlider.Fill = Brush(MainWindow.TabPlayer1Points.Content, MainWindow.TabPlayer2Points.Content);
            MainWindow.TabPlayersAvaragePointsSlider.Fill = Brush(MainWindow.TabPlayer1AvaragePoints.Content, MainWindow.TabPlayer2AvaragePoints.Content);
            MainWindow.TabPlayersAvarageHandPointsSlider.Fill = Brush(MainWindow.TabPlayer1AvarageHandPoints.Content, MainWindow.TabPlayer2AvarageHandPoints.Content);
            MainWindow.TabPlayersBestHandSlider.Fill = Brush(MainWindow.TabPlayer1BestHand.Content, MainWindow.TabPlayer2BestHand.Content);
            MainWindow.TabPlayers_180Slider.Fill = Brush(MainWindow.TabPlayer1_180.Content, MainWindow.TabPlayer2_180.Content);
            MainWindow.TabPlayersFaultThrowsSlider.Fill = Brush(MainWindow.TabPlayer1FaultThrows.Content, MainWindow.TabPlayer2FaultThrows.Content);
            MainWindow.TabPlayersTrembleThrowsSlider.Fill = Brush(MainWindow.TabPlayer1TrembleThrow.Content, MainWindow.TabPlayer2TrembleThrow.Content);
            MainWindow.TabPlayersBulleyeThrowsSlider.Fill = Brush(MainWindow.TabPlayer1BulleyeThrow.Content, MainWindow.TabPlayer2BulleyeThrow.Content);
            MainWindow.TabPlayersDoubleThrowsSlider.Fill = Brush(MainWindow.TabPlayer1DoubleThrow.Content, MainWindow.TabPlayer2DoubleThrow.Content);
            MainWindow.TabPlayersDoubleThrowsSlider.Fill = Brush(MainWindow.TabPlayer1DoubleThrow.Content, MainWindow.TabPlayer2DoubleThrow.Content);
            MainWindow.TabPlayers_25ThrowsSlider.Fill = Brush(MainWindow.TabPlayer1_25Throw.Content, MainWindow.TabPlayer2_25Throw.Content);
            MainWindow.TabPlayersSingleThrowsSlider.Fill = Brush(MainWindow.TabPlayer1SingleThrow.Content, MainWindow.TabPlayer2SingleThrow.Content);
            MainWindow.TabPlayersZeroThrowsSlider.Fill = Brush(MainWindow.TabPlayer1ZeroThrow.Content, MainWindow.TabPlayer2ZeroThrow.Content);

            MainWindow.TabGamesPlayed.Content = Convert.ToInt32(MainWindow.TabPlayer1GamesPlayed.Content) + Convert.ToInt32(MainWindow.TabPlayer2GamesPlayed.Content);
            MainWindow.TabGamesWon.Content = Convert.ToInt32(MainWindow.TabPlayer1GamesWon.Content) + Convert.ToInt32(MainWindow.TabPlayer2GamesWon.Content);
            MainWindow.TabSetsPlayed.Content = Convert.ToInt32(MainWindow.TabPlayer1SetsPlayed.Content) + Convert.ToInt32(MainWindow.TabPlayer2SetsPlayed.Content);
            MainWindow.TabSetsWon.Content = Convert.ToInt32(MainWindow.TabPlayer1SetsWon.Content) + Convert.ToInt32(MainWindow.TabPlayer2SetsWon.Content);
            MainWindow.TabLegsPlayed.Content = Convert.ToInt32(MainWindow.TabPlayer1LegsPlayed.Content) + Convert.ToInt32(MainWindow.TabPlayer2LegsPlayed.Content);
            MainWindow.TabLegsWon.Content = Convert.ToInt32(MainWindow.TabPlayer1LegsWon.Content) + Convert.ToInt32(MainWindow.TabPlayer2LegsWon.Content);
            MainWindow.TabNumberOfThrows.Content = Convert.ToInt32(MainWindow.TabPlayer1Throws.Content) + Convert.ToInt32(MainWindow.TabPlayer2Throws.Content);
            MainWindow.TabNumberOfPoints.Content = Convert.ToInt32(MainWindow.TabPlayer1Points.Content) + Convert.ToInt32(MainWindow.TabPlayer2Points.Content);
            if (Convert.ToInt32(MainWindow.TabNumberOfThrows.Content) == 0)
                MainWindow.TabAvaragePoints.Content = 0;
            else
                MainWindow.TabAvaragePoints.Content = Math.Round(Convert.ToDouble(MainWindow.TabNumberOfPoints.Content) / Convert.ToDouble(MainWindow.TabNumberOfThrows.Content), 2);
            MainWindow.TabAvarageHand.Content = Math.Round(Convert.ToDouble(MainWindow.TabAvaragePoints.Content) * 3, 2);
            MainWindow.Tab_180.Content = Convert.ToInt32(MainWindow.TabPlayer1_180.Content) + Convert.ToInt32(MainWindow.TabPlayer2_180.Content);
            MainWindow.TabFaultThrows.Content = Convert.ToInt32(MainWindow.TabPlayer1FaultThrows.Content) + Convert.ToInt32(MainWindow.TabPlayer2FaultThrows.Content);
            MainWindow.TabTrembleThrows.Content = Convert.ToInt32(MainWindow.TabPlayer1TrembleThrow.Content) + Convert.ToInt32(MainWindow.TabPlayer2TrembleThrow.Content);
            MainWindow.TabBulleyeThrows.Content = Convert.ToInt32(MainWindow.TabPlayer1BulleyeThrow.Content) + Convert.ToInt32(MainWindow.TabPlayer2BulleyeThrow.Content);
            MainWindow.TabDoubleThrows.Content = Convert.ToInt32(MainWindow.TabPlayer1DoubleThrow.Content) + Convert.ToInt32(MainWindow.TabPlayer2DoubleThrow.Content);
            MainWindow.Tab_25Throws.Content = Convert.ToInt32(MainWindow.TabPlayer1_25Throw.Content) + Convert.ToInt32(MainWindow.TabPlayer2_25Throw.Content);
            MainWindow.TabSingleThrows.Content = Convert.ToInt32(MainWindow.TabPlayer1SingleThrow.Content) + Convert.ToInt32(MainWindow.TabPlayer2SingleThrow.Content);
            MainWindow.TabZeroThrows.Content = Convert.ToInt32(MainWindow.TabPlayer1ZeroThrow.Content) + Convert.ToInt32(MainWindow.TabPlayer2ZeroThrow.Content);
        }
        static Brush Brush(object o1, object o2)   //  Кисть
        {
            double p1 = Convert.ToDouble(o1);
            double p2 = Convert.ToDouble(o2);
            double point = 0;
            if (p1 == 0 || p2 == 0)
            {
                if (p1 == 0)
                    point = 0;
                if (p2 == 0)
                    point = 1;
                if (p1 == 0 && p2 == 0)
                    point = 0.5;
            }
            else
                point = p1 / (p2 + p1);
            point = Math.Round(point, 3);
            LinearGradientBrush B = new LinearGradientBrush();
            B.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF023042"), 0));
            B.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF0095EA"), point - 0.005));
            B.GradientStops.Add(new GradientStop(Colors.Black, point));
            B.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFC300CD"), point + 0.005));
            B.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF4B0131"), 1));
            B.RelativeTransform = new RotateTransform(-0.5);
            return B;
        }
    }
}
