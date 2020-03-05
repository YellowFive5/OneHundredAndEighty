#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using OneHundredAndEightyCore.Game;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public static class Converter
    {
        public static List<Player> PlayersFromTable(DataTable playersTable)
        {
            var playersList = new List<Player>();
            foreach (DataRow playerRow in playersTable.Rows)
            {
                playersList.Add(new Player(playerRow[$"{Column.Name}"].ToString(),
                                           playerRow[$"{Column.NickName}"].ToString(),
                                           Convert.ToInt32(playerRow[$"{Column.Id}"])));
            }

            return playersList;
        }

        public static DateTime DateTimeFromString(string dateTimeStringFromDb)
        {
            return DateTime.Parse(dateTimeStringFromDb);
        }
    }
}