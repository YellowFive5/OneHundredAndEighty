using System.Text;

namespace OneHundredAndEighty
{
    public static class NewPlayer   //  Регистрация нового игрока
    {
        static MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
        public static void ShowNewPlayerRegisterWindow()    //  Показать окно регистрации
        {
            Windows.NewPlayer window = new Windows.NewPlayer();
            window.Owner = MainWindow;
            window.PlayerNameBox.Text = "Enter player name";
            window.PlayerNickNameBox.Text = "Enter player nickname";
            window.ShowDialog();
        }
        public static void ShowWelcomeWindow(string name, string nickname)  //  Показать окно после регистрации
        {
            Windows.WelcomeNewPlayer window = new Windows.WelcomeNewPlayer();
            window.NameNickName.Content = new StringBuilder().Append(name).Append(" \" ").Append(nickname).Append(" \"").ToString();
            window.Owner = MainWindow;
            window.ShowDialog();
        }
        public static void ShowExistingPlayerWindow(string name, string nickname)   //  Показать окно предупреждения о существовании игрока
        {
            Windows.PlayerExists window = new Windows.PlayerExists();
            window.ExistingPlayerName.Content = new StringBuilder().Append(name).Append(" \" ").Append(nickname).Append(" \"").ToString();
            window.Owner = MainWindow;
            window.ShowDialog();
        }
    }
}
