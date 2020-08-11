using GamesAdmin.Database;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api._Shared.Middlewares
{
    public static class RepositoryMiddleware
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<IBigSmallReportRepository, BigSmallReportRepository>();
            services.AddScoped<IStatisticRepository, StatisticRepository>();
            services.AddScoped<IBigSmallTurboReportRepository, BigSmallTurboReportRepository>();
            services.AddScoped<IOddEvenReportRepository, OddEvenReportRepository>();
            services.AddScoped<IOddEvenTurboReportRepository, OddEvenTurboReportRepository>();
            services.AddScoped<IBetHistoryReportRepository, BetHistoryReportRepository>();
            services.AddScoped<IShakeThePlateReportRepository, ShakeThePlateReportRepository>();
            services.AddScoped<IMiniSeDieReportRepository, MiniSeDieReportRepository>();
            services.AddScoped<IFishPrawnCrabReportRepository, FishPrawnCrabReportRepository>();
            services.AddScoped<IDragonTigerReportRepository, DragonTigerReportRepository>();
            services.AddScoped<IBaccaratReportRepository, BaccaratReportRepository>();
            services.AddScoped<IDailySummaryRepository, DailySummaryRepository>();
            services.AddScoped<IRouletteReportRepository, RouletteReportRepository>();
        }
    }
}