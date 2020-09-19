#region Usings

using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OneHundredAndEightyCoreWebApi.Services;

#endregion

namespace OneHundredAndEightyCoreWebApi
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; }

        public Startup(IConfiguration configuration)
        {
            AppConfiguration = configuration;
        }

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

            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("ping", Ping);
            routeBuilder.MapMiddlewareGet("uptime", appBuilder => { appBuilder.UseMiddleware<UptimeMiddleware>(); });
            app.UseRouter(routeBuilder.Build());

            app.Run(async context => { await context.Response.WriteAsync("Hello BullEyed World!"); });
        }

        private async Task Ping(HttpContext context)
        {
            await context.Response.WriteAsync("Pong !");
        }
    }
}