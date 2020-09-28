#region Usings

using Microsoft.AspNetCore.Mvc;

#endregion

namespace OneHundredAndEightyCore.WebApi.Controllers
{
    [ApiController]
    [Route("")]
    public class SayHelloController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello Bulleyed World from Web API!";
        }
    }
}