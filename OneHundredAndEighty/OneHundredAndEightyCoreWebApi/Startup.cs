#region Usings

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OneHundredAndEightyCoreWebApi.Services;

#endregion

namespace OneHundredAndEightyCoreWebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDateTimeService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.Map("/ping", Ping);

            app.Map("/uptime", Uptime);
        }

        private void Uptime(IApplicationBuilder app)
        {
            var dateTimeService = app.ApplicationServices.GetService<IDateTimeService>();
            app.Run(async context => { await context.Response.WriteAsync($"{dateTimeService.GetUptimeString()}"); });
        }

        private void Ping(IApplicationBuilder app)
        {
            app.Run(async context => { await context.Response.WriteAsync("Pong !"); });
        }
    }
}