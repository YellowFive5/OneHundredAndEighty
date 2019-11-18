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
            //
            var p1 = new Player("Player", "One", 1);
            var p2 = new Player("Player", "Two", 2);
            // dbService.SaveNewPlayer(p1);
            // dbService.SaveNewPlayer(p2);
            //
            var game = new Game(GameType.FreeThrows);
            dbService.StartNewGame(game,
                                   new List<Player>
                                   {
                                       p1,
                                       p2
                                   });

            var thr1 = new Throw(p1, game, 20, ThrowType.Double, ThrowResultativity.Ordinary, 2, 10, new PointF(1233.55f, 4442.66f), 1300);
            dbService.SaveThrow(thr1);
            var thr2 = new Throw(p2, game, 1, ThrowType.Tremble, ThrowResultativity.Fault, 1, 15, new PointF(1311.55f, 4123.66f), 1300);
            dbService.SaveThrow(thr2);

            dbService.EndGame(game);
        }
    }
}