using GamesAdmin.Site.Features.Report;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Site._Shared.Middlewares
{
    public static class ReportServiceMiddleware
    {
        public static void AddReportServices(this IServiceCollection services)
        {
            services.AddScoped<ReportView>();
            services.AddScoped<IReportServiceFactory, ReportServiceFactory>();
            services.AddScoped<SedieReportService>();
            services.AddScoped<MiniSedieReportService>();
            services.AddScoped<FishPrawnCrabReportService>();
            services.AddScoped<DragonTigerReportService>();
        }
    }
}