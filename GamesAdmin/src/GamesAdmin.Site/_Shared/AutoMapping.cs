using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
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
            CreateMap<GameConfig, Features.GameSettings.ViewModels.EditViewModel>();
            CreateMap<GameConfig, StatusItemViewModel>();
            CreateMap<GameConfig, MonitorItemViewModel>();
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
        }
    }
}