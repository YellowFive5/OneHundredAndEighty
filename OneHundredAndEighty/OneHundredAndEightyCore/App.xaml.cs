#region Usings

using System.Windows;
using CommandLine;

#endregion

namespace OneHundredAndEightyCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static bool NoCams { get; private set; }

        private class Option
        {
            [Option('n', "nocams", Required = false, HelpText = "Debug run with no cams and detection")]
            public bool NoCams { get; set; }
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Parser.Default.ParseArguments<Option>(e.Args).WithParsed<Option>(o => { NoCams = o.NoCams; });
        }
    }
}