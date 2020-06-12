#region Usings

using System.ComponentModel;

#endregion

namespace OneHundredAndEightyCore.Game
{
    public enum GameType
    {
        [Description(nameof(FreeThrowsSingle))]
        FreeThrowsSingle = 1,

        [Description(nameof(FreeThrowsDouble))]
        FreeThrowsDouble,
        [Description(nameof(Classic))] Classic
    }
}