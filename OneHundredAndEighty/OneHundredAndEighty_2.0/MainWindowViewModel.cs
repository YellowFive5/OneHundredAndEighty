#region Usings

using System.Collections.Generic;

#endregion

namespace OneHundredAndEighty_2._0
{
    public class MainWindowViewModel
    {
        private readonly MainWindow window;

        public MainWindowViewModel(MainWindow window)
        {
            this.window = window;
            var dbService = new DBService();

            dbService.SaveNewPlayer("Player", "One");
            dbService.SaveNewPlayer("Player", "Two");
            dbService.StartNewGame(GameType.FreeThrows_1, new List<string> {"One", "Two"});
        }
    }
}