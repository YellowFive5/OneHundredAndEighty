using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OneHundredAndEighty.Windows
{
    /// <summary>
    /// Логика взаимодействия для NewPlayer.xaml
    /// </summary>
    public partial class NewPlayer : Window
    {
        MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
        string PlayerName;
        string PlayerNickName;
        DateTime RegistrarionDate;

        public NewPlayer()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PlayerNameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox T = sender as TextBox;
            T.Foreground = new SolidColorBrush(Colors.Black);
            if (T.Text == "Enter player nickname" || T.Text == "Enter player name" || T.Text == "Player must have nickname!" || T.Text == "Player must have name!")
                T.Text = "";
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlayerNickNameBox.Text == "" || PlayerNickNameBox.Text == "Enter player nickname" || PlayerNameBox.Text == "" || PlayerNameBox.Text == "Enter player name" || PlayerNameBox.Text == "Player must have name!" || PlayerNickNameBox.Text == "Player must have nickname!")
            {
                if (PlayerNickNameBox.Text == "" || PlayerNickNameBox.Text == "Enter player nickname")
                {
                    PlayerNickNameBox.Foreground = new SolidColorBrush(Colors.Red);
                    PlayerNickNameBox.Text = "Player must have nickname!";
                }
                if (PlayerNameBox.Text == "" || PlayerNameBox.Text == "Enter player name")
                {
                    PlayerNameBox.Foreground = new SolidColorBrush(Colors.Red);
                    PlayerNameBox.Text = "Player must have name!";
                }
            }
            else
            {
                PlayerName = PlayerNameBox.Text;
                PlayerNickName = PlayerNickNameBox.Text;
                RegistrarionDate = DateTime.Now;
                this.Close();
                DBwork.SaveNewPlayer(PlayerName,PlayerNickName,RegistrarionDate);
                OneHundredAndEighty.NewPlayer.ShowWelcomeWindow(PlayerName,PlayerNickName);
            }
        }
    }
}
