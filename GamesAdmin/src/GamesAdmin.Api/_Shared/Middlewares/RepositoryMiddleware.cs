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
            services.AddScoped<IBigSmallTurboReportRepository, BigSmallTurboReportRepository>();
            services.AddScoped<IOddEvenReportRepository, OddEvenReportRepository>();
            services.AddScoped<IOddEvenTurboReportRepository, OddEvenTurboReportRepository>();
            services.AddScoped<IBetHistoryReportRepository, BetHistoryReportRepository>();
            services.AddScoped<ISeDieReportRepository, SeDieReportRepository>();
            services.AddScoped<IMiniSeDieReportRepository, MiniSeDieReportRepository>();
            services.AddScoped<IFishPrawnCrabReportRepository, FishPrawnCrabReportRepository>();
            services.AddScoped<IDragonTigerReportRepository, DragonTigerReportRepository>();
            services.AddScoped<IBaccaratReportRepository, BaccaratReportRepository>();
            services.AddScoped<IDailySummaryRepository, DailySummaryRepository>();
            services.AddScoped<IRouletteReportRepository, RouletteReportRepository>();
            services.AddScoped<IVipSeDieReportRepository, VipSeDieReportRepository>();
            services.AddScoped<ISpecialSeDieReportRepository, SpecialSeDieReportRepository>();
            services.AddScoped<IBlackjackReportRepository, BlackjackReportRepository>();
            services.AddScoped<IVipFishPrawnCrabReportRepository, VipFishPrawnCrabReportRepository>();
            services.AddScoped<ISicBoReportRepository, SicBoReportRepository>();
            services.AddScoped<IVipBaccaratReportRepository, VipBaccaratReportRepository>();
            services.AddScoped<IVipSeDieWarReportRepository, VipSeDieWarReportRepository>();

            services.AddScoped<IVoidLogRepository, VoidLogRepository>();
            services.AddScoped<IFourGuardiansReportRepository, FourGuardiansReportRepository>();
            services.AddScoped<ILotteryReportRepository, LotteryReportRepository>();
            services.AddScoped<IJackpotRepository, JackpotRepository>();
            services.AddScoped<IThreeCardsReportRepository, ThreeCardsReportRepository>();
            services.AddScoped<IHeroesSlotReportRepository, HeroesSlotReportRepository>();
            services.AddScoped<IBitcoinMasterReportRepository, BitcoinMasterReportRepository>();
            services.AddScoped<IKenoMatchReportRepository, KenoMatchReportRepository>();
            services.AddScoped<IKenoPowerReportRepository, KenoPowerReportRepository>();
            services.AddScoped<IMiniKenoReportRepository, MiniKenoReportRepository>();
            services.AddScoped<ICasinoHoldemPokerReportRepository, CasinoHoldemPokerReportRepository>();
        }
    }
}