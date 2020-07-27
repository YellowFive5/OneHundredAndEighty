#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class Player
    {
        #region DB

        public int Id { get; }
        public string Name { get; }
        public string NickName { get; }
        public BitmapImage Avatar { get; }

        #endregion

        #region Game

        public int SetsWon { get; set; }
        public int LegsWon { get; set; }
        public int LegPoints { get; set; }
        public int HandPoints { get; set; }
        public ThrowNumber ThrowNumber { get; set; }
        public List<Throw> HandThrows { get; set; }
        public PlayerOrder Order { get; set; }

        #endregion

        public Player(string name, string nickName, int id = -1, BitmapImage avatar = null)
        {
            Id = id;
            Name = name;
            NickName = nickName;
            Avatar = avatar ?? Converter.BitmapToBitmapImage(Resources.Resources.EmptyUserIcon);
        }

        public override string ToString()
        {
            return $"{Name} '{NickName}'";
        }

        #region IClonable

        private Player(int setsWon,
                       int legsWon,
                       int legPoints,
                       int handPoints,
                       ThrowNumber throwNumber,
                       List<Throw> handThrows,
                       PlayerOrder order)
        {
            SetsWon = setsWon;
            LegsWon = legsWon;
            LegPoints = legPoints;
            HandPoints = handPoints;
            ThrowNumber = throwNumber;
            HandThrows = handThrows;
            Order = order;
        }

        public Player Copy()
        {
            return new Player(SetsWon,
                              LegsWon,
                              LegPoints,
                              HandPoints,
                              ThrowNumber,
                              HandThrows.ToList(),
                              Order);
        }

        #endregion
    }
}