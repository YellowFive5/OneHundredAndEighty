#region Usings

using System.Windows;
using CommandLine;

#endregion

namespace OneHundredAndEightyCore
{
    public partial class App
    {
        public static bool NoCams { get; private set; }
        public static bool ThrowPanel { get; private set; }

        private class Options
        {
            [Option('n', "nocams", Required = false, HelpText = "Debug run with no cams and detection")]
            public bool NoCams { get; set; }

            [Option('p', "panel", Required = false, HelpText = "Debug run with manual throw panel")]
            public bool ThrowPanel { get; set; }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var splashScreen = new SplashScreen("Resources/SplashScreenLogo.png");
            splashScreen.Show(true, true);

            base.OnStartup(e);
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Parser.Default.ParseArguments<Options>(e.Args).WithParsed(o =>
                                                                      {
                                                                          NoCams = o.NoCams;
                                                                          ThrowPanel = o.ThrowPanel;
                                                                      });
        }
    }
}