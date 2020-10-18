#region Usings

using System.Data;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public interface IDbService
    {
        void SettingsSetValue(SettingsType name, string value);
        string SettingsGetValue(SettingsType name);
        void PlayerSaveNew(Player player);
        DataTable PlayersAllLoad();
        DataSet PlayerLoad(int playerId);
        void MigrateFrom2_0to2_1();
        void MigrateFrom2_1to2_2();
        void MigrateFrom2_2to2_3();
        void MigrateFrom2_3to2_4();
        DataTable StatisticsGetForPlayer(int playerId);
    }
}