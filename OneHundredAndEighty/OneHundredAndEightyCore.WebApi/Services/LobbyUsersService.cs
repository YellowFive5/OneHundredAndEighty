#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

#endregion

namespace OneHundredAndEightyCore.WebApi.Services
{
    public class LobbyUsersService : ILobbyUsersService
    {
        private Dictionary<string, DateTime> ActiveUsers { get; set; }
        private readonly IConfiguration configuration;
        private TimeSpan UserActiveTime => TimeSpan.FromMinutes(double.Parse(configuration["AppConfig:UserActiveTimeMinutes"]));

        public LobbyUsersService(IConfiguration configuration)
        {
            this.configuration = configuration;
            ActiveUsers = new Dictionary<string, DateTime>();
        }

        public void AddActiveUser(string userInfo)
        {
            if (!ActiveUsers.ContainsKey(userInfo))
            {
                ActiveUsers.Add(userInfo, DateTime.Now);
            }
            else if (ActiveUsers[userInfo] + UserActiveTime > DateTime.Now)
            {
                ActiveUsers[userInfo] = DateTime.Now;
            }
        }

        public List<string> GetActiveUsers()
        {
            ActiveUsers = ActiveUsers.Where(au => au.Value + UserActiveTime > DateTime.Now)
                                     .ToDictionary(au => au.Key, au => au.Value);

            return new List<string>(ActiveUsers.Keys);
        }
    }
}