#region Usings

using System.Collections.Generic;
using System.Linq;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class Hand180
    {
        public List<Throw> HandThrows { get; }
        public Player Player { get; }

        public Hand180(Player player)
        {
            Player = player;
            HandThrows = player.HandThrows.ToList();
        }
    }
}