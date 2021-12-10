using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Database.ApiLogs;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.ApiLogs
{
    public static class ApiLogInitialization
    {
        public static void InitApiLog(this IServiceCollection services)
        {
            services.AddTransient(typeof(ILogRepository<>), typeof(LogRepository<>));
        }
    }
}