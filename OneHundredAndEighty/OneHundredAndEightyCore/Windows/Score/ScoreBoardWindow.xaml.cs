#region Usings

using System.ComponentModel;
using System.Windows.Input;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public partial class ScoreBoardWindow
    {
        private bool NeedClose { get; set; }
        public ScoreBoardWindow()
        {
            InitializeComponent();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (!NeedClose)
            {
                e.Cancel = true;
            }
        }

        public void Kill()
        {
            NeedClose = true;
            Close();
        }
    }
}