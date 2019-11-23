#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using Autofac;
using OneHundredAndEighty_2._0.Recognition;

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
            var configService = MainWindow.ServiceContainer.Resolve<ConfigService>();
            
            // var _int = configService.Read<int>(SettingsType.DBVersion);
            // configService.Write(SettingsType.DBVersion, _int + 1);
            // var _int2 = configService.Read<int>(SettingsType.DBVersion);
            // var _bool = configService.Read<bool>(SettingsType.RuntimeCapturingCheckBox);
            // configService.Write(SettingsType.RuntimeCapturingCheckBox, false);
            // var _bool2 = configService.Read<bool>(SettingsType.RuntimeCapturingCheckBox);
            // var _double = configService.Read<double>(SettingsType.MoveDetectedSleepTime);
            // configService.Write(SettingsType.MoveDetectedSleepTime,1.77);
            // var _double2 = configService.Read<double>(SettingsType.MoveDetectedSleepTime);
            // var _string = configService.Read<string>(SettingsType.Cam1Id);
            // configService.Write(SettingsType.Cam1Id, "1aaa1");
            // var _string2 = configService.Read<string>(SettingsType.Cam1Id);
            //
            // var p1 = new Player("Player1", "p1");
            // var p2 = new Player("Player2", "p2");
            // dbService.SaveNewPlayer(p1);
            // dbService.SaveNewPlayer(p2);
            //
            // var game = new Game(GameType.FreeThrows);
            // dbService.StartNewGame(game,
            //                        new List<Player>
            //                        {
            //                            p1,
            //                            p2
            //                        });
            //
            // var thr1 = new Throw(p1, game, 20, ThrowType.Double, ThrowResultativity.Ordinary, 2, 10, new PointF(1233.55f, 4442.66f), 1300);
            // dbService.SaveThrow(thr1);
            // var thr2 = new Throw(p2, game, 1, ThrowType.Tremble, ThrowResultativity.Fault, 1, 15, new PointF(1311.55f, 4123.66f), 1300);
            // dbService.SaveThrow(thr2);
            //
            // dbService.EndGame(game);
        }

        public void LoadSettings()
        {
        }

        public void SaveSettings()
        {
        }
    }
}