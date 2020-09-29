#region Usings

using Microsoft.Extensions.DependencyInjection;

#endregion

namespace OneHundredAndEightyCore.WebApi.Services
{
    public static class ServiceProviderExtensions
    {
        public static void AddDateTimeService(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeService, DateTimeService>();
        }

        public static void AddLobbyUsersService(this IServiceCollection services)
        {
            services.AddSingleton<ILobbyUsersService, LobbyUsersService>();
        }
    }
}