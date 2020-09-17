#region Usings

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OneHundredAndEightyCoreWebApi.Services;

#endregion

namespace OneHundredAndEightyCoreWebApi
{
    public class UptimeMiddleware
    {
        private readonly RequestDelegate next;

        public UptimeMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context,
                                      IDateTimeService dateTimeService)
        {
            await context.Response.WriteAsync($"{dateTimeService.GetUptimeString()}");
        }
    }
}