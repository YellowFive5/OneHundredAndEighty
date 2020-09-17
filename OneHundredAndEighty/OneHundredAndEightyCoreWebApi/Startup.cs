#region Usings

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion

namespace OneHundredAndEightyCoreWebApi
{
    public class Startup
    {
        private TimeSpan AppUptime => FindUptime();

        private TimeSpan FindUptime()
        {
            return DateTime.Now - Process.GetCurrentProcess().StartTime;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.Run(async context =>
                    {
                        await context.Response.WriteAsync($"App runs for {AppUptime.Days} days, " +
                                                          $"{AppUptime.Hours} hours, " +
                                                          $"{AppUptime.Minutes} minutes and " +
                                                          $"{AppUptime.Seconds} seconds");
                    });
        }

        private void Ping(IApplicationBuilder app)
        {
            app.Run(async context => { await context.Response.WriteAsync("Pong !"); });
        }
    }
}