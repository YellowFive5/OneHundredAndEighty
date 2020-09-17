#region Usings

using Microsoft.Extensions.DependencyInjection;

#endregion

namespace OneHundredAndEightyCoreWebApi.Services
{
    public static class ServiceProviderExtensions
    {
        public static void AddDateTimeService(this IServiceCollection services)
        {
            services.AddTransient<IDateTimeService, DateTimeService>();
        }
    }
}