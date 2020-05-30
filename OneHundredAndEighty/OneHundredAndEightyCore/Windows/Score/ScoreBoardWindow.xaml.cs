#region Usings

using System.ComponentModel;
using System.Windows.Input;

#endregion

namespace OneHundredAndEightyCore.Windows.ScoreBoard
{
    public partial class ScoreBoardWindow
    {
        private bool NeedClose { get; set; }
        public ScoreBoardWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
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