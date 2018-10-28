using System;
using System.Data;
using System.Windows.Media;

namespace OneHundredAndEighty
{
    public static class PlayerOverview  //  Логика показа вкладки данных игроков
    {
        static MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Cсылка на главное окно
        public static void ClearPanel() //  Зануляем данные вкладки
        {
            ClearPlayer1();
            ClearPlayer2();
            MainWindow.Player2TabNameCombobox.SelectedIndex = -1;
            MainWindow.Player1TabNameCombobox.SelectedIndex = -1;
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
