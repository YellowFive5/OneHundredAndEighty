#region Usings

using System.Text;

#endregion

namespace OneHundredAndEighty
{
    public static class NewPlayer //  Регистрация нового игрока
    {
        private static readonly MainWindow MainWindow = (MainWindow) System.Windows.Application.Current.MainWindow; //  Ссылка на главное окно для доступа к элементам

        public static void ShowNewPlayerRegisterWindow() //  Показать окно регистрации
        {
            var window = new Windows.NewPlayer {Owner = MainWindow, PlayerNameBox = {Text = "Enter player name"}, PlayerNickNameBox = {Text = "Enter player nickname"}};
            window.ShowDialog();
        }

        public static void ShowWelcomeWindow(string name, string nickname) //  Показать окно после регистрации
        {
            var window = new Windows.WelcomeNewPlayer {NameNickName = {Content = new StringBuilder().Append(name).Append(" \" ").Append(nickname).Append(" \"").ToString()}, Owner = MainWindow};
            window.ShowDialog();
        }

        public static void ShowExistingPlayerWindow(string name, string nickname) //  Показать окно предупреждения о существовании игрока
        {
            var window = new Windows.PlayerExists {ExistingPlayerName = {Content = new StringBuilder().Append(name).Append(" \" ").Append(nickname).Append(" \"").ToString()}, Owner = MainWindow};
            window.ShowDialog();
        }
    }
}