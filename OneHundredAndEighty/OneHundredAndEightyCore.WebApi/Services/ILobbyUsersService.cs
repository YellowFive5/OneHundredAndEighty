using System.Collections.Generic;

namespace OneHundredAndEightyCore.WebApi.Services
{
    public interface ILobbyUsersService
    {
        void AddActiveUserIfNotExists(string userInfo);
        List<string> GetActiveUsers();
    }
}