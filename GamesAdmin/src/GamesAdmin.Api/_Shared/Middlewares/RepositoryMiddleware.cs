using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Baccarat;
using GamesAdmin.Database.Report;
using GamesAdmin.Database.Report.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api._Shared.Middlewares
{
    public static class RepositoryMiddleware
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<IStakeBalancedReportRepository, StakeBalancedReportRepository>();
            services.AddScoped<IBetHistoryReportRepository, BetHistoryReportRepository>();
            services.AddScoped<ISeDieReportRepository, SeDieReportRepository>();
            services.AddScoped<IMiniSeDieReportRepository, MiniSeDieReportRepository>();
            services.AddScoped<IFishPrawnCrabReportRepository, FishPrawnCrabReportRepository>();
            services.AddScoped<IDragonTigerReportRepository, DragonTigerReportRepository>();
            services.AddScoped<IRouletteReportRepository, RouletteReportRepository>();
            services.AddScoped<IVipSeDieReportRepository, VipSeDieReportRepository>();
            services.AddScoped<IBlackjackReportRepository, BlackjackReportRepository>();
            services.AddScoped<IVipFishPrawnCrabReportRepository, VipFishPrawnCrabReportRepository>();
            services.AddScoped<ISicBoReportRepository, SicBoReportRepository>();
            services.AddScoped<IVipSeDieWarReportRepository, VipSeDieWarReportRepository>();

            services.AddScoped<IVoidLogRepository, VoidLogRepository>();
            services.AddScoped<IFourGuardiansReportRepository, FourGuardiansReportRepository>();
            services.AddScoped<ILotteryReportRepository, LotteryReportRepository>();
            services.AddScoped<IJackpotRepository, JackpotRepository>();
            services.AddScoped<IThreeCardsReportRepository, ThreeCardsReportRepository>();
            services.AddScoped<IHeroesSlotReportRepository, HeroesSlotReportRepository>();
            services.AddScoped<IKenoMatchReportRepository, KenoMatchReportRepository>();
            services.AddScoped<IKenoPowerReportRepository, KenoPowerReportRepository>();
            services.AddScoped<IMiniKenoReportRepository, MiniKenoReportRepository>();
            services.AddScoped<IHiLoReportRepository, HiLoReportRepository>();
            services.AddScoped<INightClubReportRepository, NightClubReportRepository>();
            services.AddScoped<ITreasureIslandReportRepository, TreasureIslandReportRepository>();
            services.AddScoped<HoldemPokerReportRepository<CasinoHoldemPokerRoundEntity, CasinoHoldemPokerBetEntity, CasinoHoldemPokerBetReportEntity, CasinoHoldemPokerBetHistoryEntity>>();
            services.AddScoped<HoldemPokerReportRepository<TexasHoldemPokerRoundEntity, TexasHoldemPokerBetEntity, TexasHoldemPokerBetReportEntity, TexasHoldemPokerBetHistoryEntity>>();
            services.AddScoped<IUnbalancedBigSmallReportRepository, UnbalancedBigSmallReportRepository>();
            services.AddScoped<IWayToGoalReportRepository, WayToGoalReportRepository>();
            services.AddScoped<BaccaratReportRepository<BaccaratRoundEntity, BaccaratBetEntity>>();
            services.AddScoped<BaccaratReportRepository<VipBaccaratRoundEntity, VipBaccaratBetEntity>>();
            services.AddScoped<BaccaratReportRepository<BlockchainBaccaratRoundEntity, BlockchainBaccaratBetEntity>>();

            services.AddScoped<IMiniSeDieMultiplayReportRepository, MiniSeDieMultiplayReportRepository>();
        }
    }
}