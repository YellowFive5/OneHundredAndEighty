#region Usings

using Microsoft.AspNetCore.Mvc;
using OneHundredAndEightyCore.WebApi.Services;

#endregion

namespace OneHundredAndEightyCore.WebApi.Controllers
{
    [ApiController]
    [Route("uptime")]
    public class UptimeController : ControllerBase
    {
        private readonly IDateTimeService dateTimeService;

        public UptimeController(IDateTimeService dateTimeService)
        {
            this.dateTimeService = dateTimeService;
        }

        [HttpGet]
        public string GetUptime()
        {
            return dateTimeService.GetUptimeString();
        }
    }
}