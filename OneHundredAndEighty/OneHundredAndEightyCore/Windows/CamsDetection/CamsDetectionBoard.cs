#region Usings

using NLog;

#endregion

namespace OneHundredAndEightyCore.Windows.CamsDetection
{
    public class CamsDetectionBoard
    {
        private readonly Logger logger;
        private CamsDetectionWindow camsDetectionWindow;

        public CamsDetectionBoard(Logger logger)
        {
            this.logger = logger;

            camsDetectionWindow = new CamsDetectionWindow(this);

            camsDetectionWindow.Show();
        }

        public void Close()
        {
            camsDetectionWindow?.Close();
        }
    }
}