using System.Text;

namespace OneHundredAndEighty
{
    public static class NewPlayer
    {

        public static void ShowNewPlayerRegisterWindow()
        {
            MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
            Windows.NewPlayer window = new Windows.NewPlayer();
            window.Owner = MainWindow;
            window.PlayerNameBox.Text = "Enter player name";
            window.PlayerNickNameBox.Text = "Enter player nickname";
            window.ShowDialog();
        }
        public static void ShowWelcomeWindow(string name, string nickname)
        {
            MainWindow MainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);    //  Ссылка на главное окно для доступа к элементам
            Windows.WelcomeNewPlayer window = new Windows.WelcomeNewPlayer();
            window.NameNickName.Content = new StringBuilder().Append(name).Append(" \" ").Append(nickname).Append(" \"").ToString();
            window.Owner = MainWindow;
            window.ShowDialog();
        }
    }
}
