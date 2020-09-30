#region Usings

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace OneHundredAndEightyCore.WebApi.Services
{
    public class LobbyUsersService : ILobbyUsersService
    {
        private Dictionary<string, DateTime> ActiveUsers { get; set; }
        private readonly TimeSpan userActivityOffset = TimeSpan.FromMinutes(1); // todo from appSettings

        public LobbyUsersService()
        {
            ActiveUsers = new Dictionary<string, DateTime>();
        }

        public void AddActiveUser(string userInfo)
        {
            if (!ActiveUsers.ContainsKey(userInfo))
            {
                ActiveUsers.Add(userInfo, DateTime.Now);
            }
            else if (ActiveUsers[userInfo] + userActivityOffset > DateTime.Now)
            {
                ActiveUsers[userInfo] = DateTime.Now;
            }
        }

        public List<string> GetActiveUsers()
        {
            ActiveUsers = ActiveUsers.Where(au => au.Value + userActivityOffset > DateTime.Now)
                                     .ToDictionary(au => au.Key, au => au.Value);

            return new List<string>(ActiveUsers.Keys);
        }
    }
}