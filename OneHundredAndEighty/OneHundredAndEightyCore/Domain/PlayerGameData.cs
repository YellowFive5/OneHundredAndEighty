#region Usings

using System.Collections.Generic;
using System.Linq;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class PlayerGameData
    {
        public int PlayerId { get; set; }
        public int SetsWon { get; set; }
        public int LegsWon { get; set; }
        public int LegPoints { get; set; }
        public int HandPoints { get; set; }
        public ThrowNumber ThrowNumber { get; set; }
        public List<Throw> HandThrows { get; set; }
        public PlayerOrder Order { get; set; }

        public PlayerGameData Copy()
        {
            return new PlayerGameData
                   {
                       PlayerId = PlayerId,
                       SetsWon = SetsWon,
                       LegsWon = LegsWon,
                       LegPoints = LegPoints,
                       HandPoints = HandPoints,
                       ThrowNumber = ThrowNumber,
                       HandThrows = HandThrows.ToList(),
                       Order = Order
                   };
        }
    }
}