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
            services.AddScoped<BaccaratReportService>();
            services.AddScoped<RouletteReportService>();
            services.AddScoped<VipSeDieReportService>();
            services.AddScoped<SicBoReportService>();
            services.AddScoped<VipFishPrawnCrabReportService>();
            services.AddScoped<LotteryReportService>();
            services.AddScoped<ThreeCardsReportService>();
            services.AddScoped<KenoMatchReportService>();
            services.AddScoped<KenoPowerReportService>();
            services.AddScoped<MiniKenoReportService>();
            services.AddScoped<VipSeDieWarReportService>();
            services.AddScoped<HiLoReportService>();
            services.AddScoped<MiniSedieReportService>();
            services.AddScoped<UnbalancedBigSmallReportService>();
            services.AddScoped<SlotRNGReportService>();
            services.AddScoped<FanTanReportService>();
            services.AddScoped<WinThreeCardsReportService>();
            services.AddScoped<DragonTigerFightReportService>();
            services.AddScoped<ColorGameReportService>();
            services.AddScoped<BandarQiuQiuReportService>();
            services.AddScoped<BacBall1ReportService>();
            services.AddScoped<BacBall2ReportService>();
            services.AddScoped<NextBallReportService>();
        }
    }
}