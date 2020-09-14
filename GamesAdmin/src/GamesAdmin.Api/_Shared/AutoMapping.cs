using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamesAdmin.Api.Notifications.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.Spins;
using GamesAdmin.Core.Models.Um;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Spins;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Bson;

namespace GamesAdmin.Api._Shared
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<List<ObjectId>, List<string>>().ConvertUsing(o => o.Select(os => os.ToString()).ToList());
            CreateMap<List<string>, List<ObjectId>>().ConvertUsing(o => o.Select(os => ObjectId.Parse(os)).ToList());
            CreateMap<ObjectId, string>().ConvertUsing(o => o.ToString());
            CreateMap<string, ObjectId>().ConvertUsing(s => ObjectId.Parse(s));

            CreateMap<GameConfigEntity, GameConfig>()
                 .ForMember(dest => dest.DisabledRound, opt => opt.MapFrom(src => src.Disabledround));

            CreateMap<GameConfig, GameConfigEntity>();

            CreateMap<BigSmallRoundEntity, Round>();
            CreateMap<BigSmallOnesEntity, BetInfo>();

            CreateMap<User, AdminUserEntity>();
            CreateMap<UmSchedule, EventScheduleEntity>()
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start.UtcDateTime))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End.UtcDateTime))
                .ReverseMap();

            CreateMap<AddRequest, NotificationEntity>()
                .ForMember(dest => dest.DisplayKind, opt => opt.MapFrom(src => byte.Parse(src.DisplayKind.Value)));

            CreateMap<UpdateRequest, NotificationEntity>()
                .ForMember(dest => dest.DisplayKind, opt => opt.MapFrom(src => byte.Parse(src.DisplayKind.Value)));

            CreateMap<Notification, NotificationEntity>()
                .ForMember(dest => dest.DisplayKind, opt => opt.MapFrom(src => byte.Parse(src.DisplayKind.Value)));

            CreateMap<FishPrawnCrabRoundEntity, FishPrawnCrabRoundResult>();
            CreateMap<FishPrawnCrabBetReportEntity, FishPrawnCrabBetRecord>();

            CreateMap<Card, CardResult>();
            CreateMap<DragonTigerRoundEntity, DragonTigerRoundResult>();
            CreateMap<DragonTigerBetReportEntity, DragonTigerBetRecord>();

            CreateMap<BaccaratSettlementResultEntity, BaccaratSettlementResult>();
            CreateMap<BaccaratBetReportEntity, BaccaratBetRecord>();
            CreateMap<BaccaratRoundEntity, BaccaratRoundResult>()
              .ForMember(x => x.PlayerCards, opt => opt.MapFrom((source, destination, i, context) =>
              {
                  if (source.Player != null)
                  {
                      var mapper = context.Mapper;
                      var result = new List<CardResult> { mapper.Map<CardResult>(source.Player.FirstCard), mapper.Map<CardResult>(source.Player.SecondCard) };
                      if (source.Player.ThirdCard != null)
                      {
                          result.Add(mapper.Map<CardResult>(source.Player.ThirdCard));
                      }

                      return result;
                  }

                  return Enumerable.Empty<CardResult>();
              }))
              .ForMember(x => x.BankerCards, opt => opt.MapFrom((source, destination, i, context) =>
              {
                  if (source.Banker != null)
                  {
                      var mapper = context.Mapper;
                      var result = new List<CardResult> { mapper.Map<CardResult>(source.Banker.FirstCard), mapper.Map<CardResult>(source.Banker.SecondCard) };
                      if (source.Banker.ThirdCard != null)
                      {
                          result.Add(mapper.Map<CardResult>(source.Banker.ThirdCard));
                      }

                      return result;
                  }

                  return Enumerable.Empty<CardResult>();
              }));

            CreateMap<RouletteBetReportEntity, RouletteBetRecord>();
            CreateMap<RouletteRoundEntity, RouletteRoundResult>();
            CreateMap<RouletteSettlementResultEntity, RouletteSettlementResult>();
            CreateMap<RouletteBetEntity, RouletteBetHistoryRecord>();

            CreateMap<SpinResultEntity, SpinResult>();

            CreateMap<VipSeDieRoundEntity, SeDieRoundResult>()
                .ForMember(x => x.Chips, opt => opt.MapFrom((source) => new bool[]
                {
                    source.Result.FirstRedChip,
                    source.Result.FirstRedChip,
                    source.Result.FirstRedChip,
                    source.Result.FirstRedChip
                }));

            CreateMap<VipSeDieBetReportEntity, VipSeDieBetRecord>();
        }
    }
}