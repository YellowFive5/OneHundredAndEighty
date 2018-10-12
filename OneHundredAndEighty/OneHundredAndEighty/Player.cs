using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace OneHundredAndEighty
{
    public class Player
    {
        public string Name { get; private set; }
        public string Tag { get; private set; }
        public bool OnThrow = false;
        public bool Legstart = false;
        public int LegsWon = 0;
        public int SetsWon = 0;
        public int PointsToOut = 0;

        public int HandPoints = 0;
        public int? FirstThrow = null;
        public int? SecondThrow = null;
        public int? ThirdThrow = null;

        public Canvas HelpPanel { get; private set; }
        public Label HelpLabel { get; set; }
        public Label SetsWonLabel { get; private set; }
        public Label LegsWonLabel { get; private set; }
        public Label PointsLabel { get; private set; }
        public Ellipse DotPoint { get; private set; }

        public Player(string Tag, string Name, Canvas HelpPanel, Label HelpLabel, Label SetsWonLabel, Label LegsWonLabel, Label PointsLabel, Ellipse DotPoint, int PointsToOut)
        {
            this.Tag = Tag;
            this.Name = Name;
            this.HelpPanel = HelpPanel;
            this.HelpLabel = HelpLabel;
            this.SetsWonLabel = SetsWonLabel;
            this.LegsWonLabel = LegsWonLabel;
            this.PointsLabel = PointsLabel;
            this.DotPoint = DotPoint;
            this.PointsToOut = PointsToOut;
        }
    }
}
