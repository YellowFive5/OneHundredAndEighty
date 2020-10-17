#region Usings

using System.Windows.Media.Imaging;
using OneHundredAndEightyCore.Common;

#endregion

namespace OneHundredAndEightyCore.Domain
{
    public class Player
    {
        public int Id { get; }
        public string Name { get; }
        public string NickName { get; }
        public BitmapImage Avatar { get; }

        public PlayerStatistics Statistics  { get; } // todo 
        public PlayerGameData GameData { get; }

        public Player(string name, string nickName, BitmapImage avatar = null, int id = -1)
        {
            Id = id;
            Name = name;
            NickName = nickName;
            Avatar = avatar ?? Converter.BitmapToBitmapImage(Resources.Resources.EmptyUserIcon);

            GameData = new PlayerGameData {PlayerId = Id};
        }

        public override string ToString()
        {
            return $"{Name} '{NickName}'";
        }
    }
}