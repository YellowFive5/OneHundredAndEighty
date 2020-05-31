#region Usings

#endregion

using System.ComponentModel;

namespace OneHundredAndEightyCore.Windows.CamsDetection
{
    public partial class CamsDetectionWindow
    {
        private readonly CamsDetectionBoard camsDetectionViewModel;

        public CamsDetectionWindow(CamsDetectionBoard camsDetectionViewModel)

        {
            this.camsDetectionViewModel = camsDetectionViewModel;
            InitializeComponent();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            camsDetectionViewModel.SaveSettings();
        }
    }
}