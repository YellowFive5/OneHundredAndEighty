#region Usings

using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class Player : ICloneable
    {
        #region DB

        public int Id { get; }
        public string Name { get; }
        public string NickName { get; }
        public BitmapImage Avatar { get; }

        #endregion

        #region Game

        public int SetsWon { get; set; } = 0;
        public int LegsWon { get; set; } = 0;
        public int LegPoints { get; set; } = 0;
        public int HandPoints { get; set; } = 0;
        public ThrowNumber ThrowNumber { get; set; } = ThrowNumber.FirstThrow;
        public Stack<Throw> HandThrows { get; set; } = new Stack<Throw>();
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
                       Stack<Throw> handThrows,
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

        public object Clone()
        {
            return new Player(SetsWon,
                              LegsWon,
                              LegPoints,
                              HandPoints,
                              ThrowNumber,
                              HandThrows,
                              Order);
        }

        #endregion
    }
}