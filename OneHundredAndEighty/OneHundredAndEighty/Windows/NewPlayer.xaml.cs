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
        string PlayerName;  //  Имя нового игрока
        string PlayerNickName;  //  Ник нового игрока

        public NewPlayer()
        {
            InitializeComponent();
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e) //  Кнопка выхода
        {
            this.Close();
        }
        private void PlayerNameBox_GotFocus(object sender, RoutedEventArgs e)   //  Фокус на текстбоксе
        {
            TextBox T = sender as TextBox;
            T.Foreground = new SolidColorBrush(Colors.Black);
            if (T.Text == "Enter player nickname" || T.Text == "Enter player name" || T.Text == "Player must have nickname!" || T.Text == "Player must have name!") //  Убираем пояснения
                T.Text = "";
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e) //  Кнопка регистрации нового игрока
        {
            //  Поля неправильно заполнены
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
            //  Поля правильно заполнены
            else
            {
                //  Сохраняем имя и ник
                PlayerName = PlayerNameBox.Text;
                PlayerNickName = PlayerNickNameBox.Text;
                this.Close();   //  Закрываем окно регистрации
                if (DBwork.IsPlayerExist(PlayerName, PlayerNickName))   //  Если в базе уже есть игрок с тиким именем и фамилией
                {
                    OneHundredAndEighty.NewPlayer.ShowExistingPlayerWindow(PlayerName,PlayerNickName);  //  Показываем предупреждение
                    OneHundredAndEighty.NewPlayer.ShowNewPlayerRegisterWindow();    //  Заново открываем окно регистрации
                }
                else  //    Игрока в базе нет   -   можно сохранять
                {
                    DBwork.SaveNewPlayer(PlayerName, PlayerNickName);   //  Сохраняем игрока
                    OneHundredAndEighty.NewPlayer.ShowWelcomeWindow(PlayerName, PlayerNickName);    //  Показываем окно приветствия
                }
            }
        }
    }
}
