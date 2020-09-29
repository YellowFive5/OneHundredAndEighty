#region Usings

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneHundredAndEightyCore.WebApi.Services;

#endregion

namespace OneHundredAndEightyCore.WebApi.Controllers
{
    [ApiController]
    [Route("lobby")]
    public class LobbyController : ControllerBase
    {
        private readonly ILobbyUsersService lobbyUsersService;

        public LobbyController(ILobbyUsersService lobbyUsersService)
        {
            this.lobbyUsersService = lobbyUsersService;
        }

        [HttpGet]
        [Route("getActiveUsers")]
        public async Task<ActionResult<List<string>>> GetActiveUsers()
        {
            return new ObjectResult(lobbyUsersService.GetActiveUsers());
        }

        [HttpPost]
        [Route("addActiveUser")]
        public async Task<ActionResult<string>> AddActiveUserIfNotExists(string userInfo)
        {
            if (userInfo == null)
            {
                return BadRequest();
            }

            lobbyUsersService.AddActiveUserIfNotExists(userInfo);
            return Ok(userInfo);
        }
    }
}