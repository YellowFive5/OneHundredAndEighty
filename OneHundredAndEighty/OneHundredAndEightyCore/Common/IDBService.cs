#region Usings

using System.Data;
using OneHundredAndEightyCore.Game;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public interface IDBService
    {
        void SettingsSetValue(SettingsType name, string value);
        string SettingsGetValue(SettingsType name);
        void PlayerSaveNew(Player player);
        DataTable PlayersLoadAll();
    }
}