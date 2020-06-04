#region Usings

using System.ComponentModel;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Windows.CamsDetection
{
    public partial class CamsDetectionWindow
    {
        private readonly CamsDetectionBoard camsDetectionViewModel;
        private bool ForceClose { get; set; }

        public CamsDetectionWindow(WindowSettings settings, CamsDetectionBoard camsDetectionViewModel)

        {
            this.camsDetectionViewModel = camsDetectionViewModel;
            InitializeComponent();

            Left = settings.PositionLeft;
            Top = settings.PositionTop;
            Height = settings.Height;
            Width = settings.Width;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (!ForceClose)
            {
                e.Cancel = true;
            }
        }

        public new void Close()
        {
            ForceClose = true;
            base.Close();
        }
    }
}