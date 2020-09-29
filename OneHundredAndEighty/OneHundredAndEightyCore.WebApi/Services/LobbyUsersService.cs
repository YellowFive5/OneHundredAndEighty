#region Usings

using System.Collections.Generic;

#endregion

namespace OneHundredAndEightyCore.WebApi.Services
{
    public class LobbyUsersService : ILobbyUsersService
    {
        public LobbyUsersService()
        {
            ActiveUsers = new List<string>();
        }

        private List<string> ActiveUsers { get; }

        public void AddActiveUserIfNotExists(string userInfo)
        {
            if (!ActiveUsers.Contains(userInfo))
            {
                ActiveUsers.Add(userInfo);
            }
        }

        public List<string> GetActiveUsers()
        {
            return ActiveUsers;
        }
    }
}