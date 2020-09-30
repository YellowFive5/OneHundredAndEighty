using System.Collections.Generic;

namespace OneHundredAndEightyCore.WebApi.Services
{
    public interface ILobbyUsersService
    {
        void AddActiveUser(string userInfo);
        List<string> GetActiveUsers();
    }
}