using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OneHundredAndEighty
{
    class Player
    {
        public Player(string Name)
        {
            this.Name = Name;

        }
        public string Name { get; private set; }
    }
}
