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

            // var p1 = new Player("Player", "One");
            // var p2 = new Player("Player", "Two");
            // dbService.SaveNewPlayer(p1);
            // dbService.SaveNewPlayer(p2);
            //
            // dbService.StartNewGame(GameType.FreeThrows,
            //                        new List<Player>
            //                        {
            //                            p1,
            //                            p2
            //                        });
        }
    }
}