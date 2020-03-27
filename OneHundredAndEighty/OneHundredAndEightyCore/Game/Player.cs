#region Usings

using System.Collections.Generic;
using System.Windows.Media.Imaging;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Game
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

        public int HandPoints { get; set; } = 0;
        public int LegPoints { get; set; } = 0;
        public int ThrowNumber { get; set; } = 1;
        public Stack<Throw> HandThrows { get; set; } = new Stack<Throw>();

        #endregion

        public Player(string name, string nickName, int id = -1, BitmapImage avatar = null)
        {
            Id = id;
            Name = name;
            NickName = nickName;
            Avatar = avatar ?? Converter.BitmapToBitmapImage(Resources.EmptyUserIcon);
        }

        public override string ToString()
        {
            return $"{Name} '{NickName}'";
        }
    }
}