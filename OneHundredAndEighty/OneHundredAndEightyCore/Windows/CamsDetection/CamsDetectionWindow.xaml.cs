#region Usings

#endregion

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
    }
}