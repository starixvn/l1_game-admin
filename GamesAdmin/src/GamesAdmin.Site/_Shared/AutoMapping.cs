using System.Collections.Generic;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Core.Models.Um;
using GamesAdmin.Site.Features.Dashboard.ViewModels;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using GamesAdmin.Site.Features.Notifications.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using GamesAdmin.Site.Features.UmSchedule.Requests;
using GamesAdmin.Site.Features.UmSchedule.ViewModels;

namespace GamesAdmin.Site._Shared
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<AutoRNGGameConfig, Features.GameSettings.ViewModels.EditViewModel>();
            CreateMap<AutoRNGGameConfig, StatusItemViewModel>();
            CreateMap<AutoRNGGameConfig, MonitorItemViewModel>();
            CreateMap<Features.GameSettings.ViewModels.AddViewModel, Features.GameSettings.Requests.SlotsConfigRequest>()
                .ForMember(dest => dest.Stakes, opt => opt.MapFrom(src => ConvertStakeLevels(src.Stakes, src.Jackpots)));
            CreateMap<Features.GameSettings.ViewModels.AddViewModel, GameInfo>();
            CreateMap<Features.GameSettings.ViewModels.AddViewModel, EnableSettings>();
            CreateMap<Features.GameSettings.ViewModels.AddViewModel, BotSettings>();
            CreateMap<Features.GameSettings.ViewModels.AddViewModel, BetLimit>();

            CreateMap<OnlineUser, OnlineUserViewModel>();
            CreateMap<BotUser, BotUserViewModel>();

            CreateMap<CreateRequest, UmSchedule>();
            CreateMap<UmSchedule, UmScheduleItemViewModel>();

            CreateMap<AddRequest, Notification>();
            CreateMap<UpdateRequest, Notification>();
            CreateMap<FishPrawnCrabRoundResult, FishPrawnCrabRoundResultViewModel>();
            CreateMap<FishPrawnCrabBetRecord, FishPrawnCrabRecordViewModel>();

            CreateMap<DragonTigerRoundResult, DragonTigerRoundResultViewModel>();
            CreateMap<DragonTigerBetRecord, DragonTigerRecordViewModel>();
            CreateMap<CardResult, CardViewModel>();

            CreateMap<BaccaratRoundResult, BaccaratRoundResultViewModel>();
            CreateMap<BaccaratSettlementResult, BaccaratSettlementResultViewModel>();
            CreateMap<BaccaratBetRecord, BaccaratRecordViewModel>();

            CreateMap<RouletteRoundResult, RouletteRoundResultViewModel>();
            CreateMap<RouletteSettlementResult, RouletteSettlementResultViewModel>();
            CreateMap<RouletteBetRecord, RouletteRecordViewModel>();
            CreateMap<RouletteBetHistoryRecord, RouletteBetHistoryRecordViewModel>();

            CreateMap<VipSeDieBetRecord, VipSeDieRecordViewModel>();
            CreateMap<SeDieRoundResult, SeDieRoundResultViewModel>();
            CreateMap<SeDieBetRecord, SeDieRecordViewModel>();

            CreateMap<SpecialSeDieBetRecord, SpecialSeDieRecordViewModel>();
            CreateMap<SpecialSeDieBetHistoryRecord, SpecialSeDieBetHistoryRecordViewModel>();

            CreateMap<BlackjackBetRecord, BlackjackRecordViewModel>();
            CreateMap<BlackjackCard, BlackjackCardViewModel>();

            CreateMap<VipFishPrawnCrabRoundResult, VipFishPrawnCrabRoundResultViewModel>()
                .ForMember(x => x.Result, opt => opt.MapFrom((source) => new[]
                {
                    source.FirstDice,
                    source.SecondDice,
                    source.ThirdDice
                }));
            CreateMap<VipFishPrawnCrabBetRecord, VipFishPrawnCrabRecordViewModel>();

            CreateMap<SicBoRoundResult, SicBoRoundResultViewModel>().ForMember(x => x.Result, opt => opt.MapFrom((source) => new[]
            {
                source.FirstDice,
                source.SecondDice,
                source.ThirdDice
            }));
            CreateMap<SicBoBetRecord, SicBoRecordViewModel>();
            CreateMap<SicBoBetHistoryRecord, SicBoBetHistoryRecordViewModel>();
            
            CreateMap<VipSeDieWarRoundResult, VipSeDieWarRoundResultViewModel>();
            CreateMap<VipSeDieWarBetRecord, VipSeDieWarRecordViewModel>();
            CreateMap<VipSeDieWarBetHistoryRecord, VipSeDieWarBetHistoryRecordViewModel>();

        }

        private static List<StakeLevel> ConvertStakeLevels(double[] stakes, double[] jackpots)
        {
            var stakeLevels = new List<StakeLevel>();

            for (var i = 0; i < stakes.Length; i++)
            {
                stakeLevels.Add(new StakeLevel(stakes[i], jackpots[i]));
            }

            return stakeLevels;
        }
    }
}
