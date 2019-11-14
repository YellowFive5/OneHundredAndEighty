#region Usings

using System.Collections.Generic;
using System.Drawing;

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

            // var version = dbService.GetSettingsValue(SettingsType.DBVersion);
            // dbService.SaveSettingsValue(SettingsType.DBVersion,2);

            // var p1 = new Player("Player", "One");
            // var p2 = new Player("Player", "Two");
            // dbService.SaveNewPlayer(p1);
            // dbService.SaveNewPlayer(p2);

            // dbService.StartNewGame(GameType.FreeThrows,
            //                        new List<Player>
            //                        {
            //                            p1,
            //                            p2
            //                        });

            // var thr1 = new Throw(1, p1, 1, 20, ThrowType.Single, ThrowResultativity.Ordinary, 1, 20, new PointF(1233.55f, 4442.66f), 1300);
            // dbService.SaveThrow(thr1);
            // var thr2 = new Throw(1, p2, 1, 1, ThrowType.Tremble, ThrowResultativity.Fault, 1, 3, new PointF(1311.55f, 4123.66f), 1300);
            // dbService.SaveThrow(thr2);
        }
    }
}