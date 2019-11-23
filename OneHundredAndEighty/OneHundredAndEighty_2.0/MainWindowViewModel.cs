#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows;
using Autofac;
using NLog;
using OneHundredAndEighty_2._0.Recognition;

#endregion

namespace OneHundredAndEighty_2._0
{
    public class MainWindowViewModel
    {
        private readonly MainWindow mainWindow;
        private readonly Logger logger;
        private readonly DBService dbService;
        private readonly ConfigService configService;

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            logger = MainWindow.ServiceContainer.Resolve<Logger>();
            dbService = MainWindow.ServiceContainer.Resolve<DBService>();
            configService = MainWindow.ServiceContainer.Resolve<ConfigService>();

            // var _int = configService.Read<int>(SettingsType.DBVersion);
            // configService.Write(SettingsType.DBVersion, _int + 1);
            // var _int2 = configService.Read<int>(SettingsType.DBVersion);
            // var _bool = configService.Read<bool>(SettingsType.RuntimeCapturingCheckBox);
            // configService.Write(SettingsType.RuntimeCapturingCheckBox, false);
            // var _bool2 = configService.Read<bool>(SettingsType.RuntimeCapturingCheckBox);
            // var _double = configService.Read<double>(SettingsType.MoveDetectedSleepTime);
            // configService.Write(SettingsType.MoveDetectedSleepTime,1.99);
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
            logger.Debug("Load settings start");

            mainWindow.Left = configService.Read<double>(SettingsType.MainWindowPositionLeft);
            mainWindow.Top = configService.Read<double>(SettingsType.MainWindowPositionTop);
            mainWindow.CamFovTextBox.Text = configService.Read<int>(SettingsType.CamFovAngle).ToString();
            mainWindow.CamResolutionHeightTextBox.Text = configService.Read<int>(SettingsType.ResolutionHeight).ToString();
            mainWindow.CamResolutionWidthTextBox.Text = configService.Read<int>(SettingsType.ResolutionWidth).ToString();
            mainWindow.MovesExtractionTextBox.Text = configService.Read<int>(SettingsType.MovesExtraction).ToString();
            mainWindow.MoveDetectedSleepTimeTextBox.Text = configService.Read<double>(SettingsType.MoveDetectedSleepTime).ToString(CultureInfo.InvariantCulture);
            mainWindow.MovesNoiseTextBox.Text = configService.Read<int>(SettingsType.MovesNoise).ToString();
            mainWindow.SmoothGaussTextBox.Text = configService.Read<int>(SettingsType.SmoothGauss).ToString();
            mainWindow.ThresholdSleepTimeTimeTextBox.Text = configService.Read<double>(SettingsType.ThresholdSleepTime).ToString(CultureInfo.InvariantCulture);
            mainWindow.ExtractionSleepTimeTimeTextBox.Text = configService.Read<double>(SettingsType.ExtractionSleepTime).ToString(CultureInfo.InvariantCulture);
            mainWindow.MinContourArcTextBox.Text = configService.Read<int>(SettingsType.MinContourArc).ToString();
            mainWindow.MovesDartTextBox.Text = configService.Read<int>(SettingsType.MovesDart).ToString();
            mainWindow.Cam1IdTextBox.Text = configService.Read<string>(SettingsType.Cam1Id);
            mainWindow.Cam2IdTextBox.Text = configService.Read<string>(SettingsType.Cam2Id);
            mainWindow.Cam3IdTextBox.Text = configService.Read<string>(SettingsType.Cam3Id);
            mainWindow.Cam4IdTextBox.Text = configService.Read<string>(SettingsType.Cam4Id);
            mainWindow.ToCam1Distance.Text = configService.Read<double>(SettingsType.ToCam1Distance).ToString(CultureInfo.InvariantCulture);
            mainWindow.ToCam2Distance.Text = configService.Read<double>(SettingsType.ToCam2Distance).ToString(CultureInfo.InvariantCulture);
            mainWindow.ToCam3Distance.Text = configService.Read<double>(SettingsType.ToCam3Distance).ToString(CultureInfo.InvariantCulture);
            mainWindow.ToCam4Distance.Text = configService.Read<double>(SettingsType.ToCam4Distance).ToString(CultureInfo.InvariantCulture);
            mainWindow.Cam1XTextBox.Text = configService.Read<int>(SettingsType.Cam1X).ToString();
            mainWindow.Cam2XTextBox.Text = configService.Read<int>(SettingsType.Cam2X).ToString();
            mainWindow.Cam3XTextBox.Text = configService.Read<int>(SettingsType.Cam3X).ToString();
            mainWindow.Cam4XTextBox.Text = configService.Read<int>(SettingsType.Cam4X).ToString();
            mainWindow.Cam1YTextBox.Text = configService.Read<int>(SettingsType.Cam1Y).ToString();
            mainWindow.Cam2YTextBox.Text = configService.Read<int>(SettingsType.Cam2Y).ToString();
            mainWindow.Cam3YTextBox.Text = configService.Read<int>(SettingsType.Cam3Y).ToString();
            mainWindow.Cam4YTextBox.Text = configService.Read<int>(SettingsType.Cam4Y).ToString();

            logger.Debug("Load settings end");
        }

        public void SaveSettings()
        {
            logger.Debug("Save settings start");

            configService.Write(SettingsType.MainWindowPositionLeft, mainWindow.Left);
            configService.Write(SettingsType.MainWindowPositionTop, mainWindow.Top);
            configService.Write(SettingsType.CamFovAngle, mainWindow.CamFovTextBox.Text);
            configService.Write(SettingsType.ResolutionHeight, mainWindow.CamResolutionHeightTextBox.Text);
            configService.Write(SettingsType.ResolutionWidth, mainWindow.CamResolutionWidthTextBox.Text);
            configService.Write(SettingsType.MovesExtraction, mainWindow.MovesExtractionTextBox.Text);
            configService.Write(SettingsType.MoveDetectedSleepTime, mainWindow.MoveDetectedSleepTimeTextBox.Text);
            configService.Write(SettingsType.MovesNoise, mainWindow.MovesNoiseTextBox.Text);
            configService.Write(SettingsType.SmoothGauss, mainWindow.SmoothGaussTextBox.Text);
            configService.Write(SettingsType.ThresholdSleepTime, mainWindow.ThresholdSleepTimeTimeTextBox.Text);
            configService.Write(SettingsType.ExtractionSleepTime, mainWindow.ExtractionSleepTimeTimeTextBox.Text);
            configService.Write(SettingsType.MinContourArc, mainWindow.MinContourArcTextBox.Text);
            configService.Write(SettingsType.MovesDart, mainWindow.MovesDartTextBox.Text);
            configService.Write(SettingsType.Cam1Id, mainWindow.Cam1IdTextBox.Text);
            configService.Write(SettingsType.Cam2Id, mainWindow.Cam2IdTextBox.Text);
            configService.Write(SettingsType.Cam3Id, mainWindow.Cam3IdTextBox.Text);
            configService.Write(SettingsType.Cam4Id, mainWindow.Cam4IdTextBox.Text);
            configService.Write(SettingsType.ToCam1Distance, mainWindow.ToCam1Distance.Text);
            configService.Write(SettingsType.ToCam2Distance, mainWindow.ToCam2Distance.Text);
            configService.Write(SettingsType.ToCam3Distance, mainWindow.ToCam3Distance.Text);
            configService.Write(SettingsType.ToCam4Distance, mainWindow.ToCam4Distance.Text);
            configService.Write(SettingsType.Cam1X, mainWindow.Cam1XTextBox.Text);
            configService.Write(SettingsType.Cam2X, mainWindow.Cam2XTextBox.Text);
            configService.Write(SettingsType.Cam3X, mainWindow.Cam3XTextBox.Text);
            configService.Write(SettingsType.Cam4X, mainWindow.Cam4XTextBox.Text);
            configService.Write(SettingsType.Cam1Y, mainWindow.Cam1YTextBox.Text);
            configService.Write(SettingsType.Cam2Y, mainWindow.Cam2YTextBox.Text);
            configService.Write(SettingsType.Cam3Y, mainWindow.Cam3YTextBox.Text);
            configService.Write(SettingsType.Cam4Y, mainWindow.Cam4YTextBox.Text);

            logger.Debug("Save settings end");
        }

        public void CheckVersion(double appVersion)
        {
            var dbVersion = configService.Read<double>(SettingsType.DBVersion);
            if (appVersion != dbVersion)
            {
                var errorText = Properties.Resources.ResourceManager.GetString("VersionsMistmatchErrorText");
                MessageBox.Show(errorText, "Error", MessageBoxButton.OK);
                throw new Exception("DB version and App version is different");
            }
        }
    }
}