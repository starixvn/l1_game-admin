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
                .ForMember(x => x.Chips, opt => opt.MapFrom((source) => new[]
                {
                    source.Result.FirstRedChip,
                    source.Result.SecondRedChip,
                    source.Result.ThirdRedChip,
                    source.Result.FourthRedChip
                }))
                .ForMember(x => x.Odd, opt => opt.MapFrom((source) => source.SettlementResult.Odd))
                .ForMember(x => x.Even, opt => opt.MapFrom((source) => source.SettlementResult.Even))
                .ForMember(x => x.TwoRed, opt => opt.MapFrom((source) => source.SettlementResult.TwoRed))
                .ForMember(x => x.ThreeRed, opt => opt.MapFrom((source) => source.SettlementResult.ThreeRed))
                .ForMember(x => x.FourRed, opt => opt.MapFrom((source) => source.SettlementResult.FourRed))
                .ForMember(x => x.ThreeWhite, opt => opt.MapFrom((source) => source.SettlementResult.ThreeWhite))
                .ForMember(x => x.FourWhite, opt => opt.MapFrom((source) => source.SettlementResult.FourWhite));

            CreateMap<VipSeDieBetReportEntity, VipSeDieBetRecord>();

            CreateMap<SpecialSeDieRoundEntity, SeDieRoundResult>()
              .ForMember(x => x.Chips, opt => opt.MapFrom((source) => new[]
              {
                    source.Result.FirstRedChip,
                    source.Result.SecondRedChip,
                    source.Result.ThirdRedChip,
                    source.Result.FourthRedChip
              }))
              .ForMember(x => x.Odd, opt => opt.MapFrom((source) => source.SettlementResult.Odd))
              .ForMember(x => x.Even, opt => opt.MapFrom((source) => source.SettlementResult.Even));
            CreateMap<SpecialSeDieBetReportEntity, SpecialSeDieBetRecord>();

            CreateMap<SpecialSeDieBetEntity, SpecialSeDieBetHistoryRecord>();

            CreateMap<BlackjackBetReportEntity, BlackjackBetRecord>();
            CreateMap<BlackjackCardEntity, BlackjackCard>();

            CreateMap<VipFishPrawnCrabRoundEntity, VipFishPrawnCrabRoundResult>()
                .ForMember(x => x.FirstDice, opt => opt.MapFrom((source) => source.Result.FirstDice))
                .ForMember(x => x.SecondDice, opt => opt.MapFrom((source) => source.Result.SecondDice))
                .ForMember(x => x.ThirdDice, opt => opt.MapFrom((source) => source.Result.ThirdDice));

            CreateMap<VipFishPrawnCrabBetReportEntity, VipFishPrawnCrabBetRecord>();
            
            CreateMap<VipBaccaratRoundEntity, BaccaratRoundResult>()
                .ForMember(x => x.Result, opt => opt.MapFrom((source) => source.SettlementResult))
                .ForMember(x => x.PlayerCards, opt => opt.MapFrom((source, destination, i, context) =>
                {
                    if (source.Result != null && source.Result.Player != null)
                    {
                        var mapper = context.Mapper;
                        var result = new List<CardResult> { mapper.Map<CardResult>(source.Result.Player.FirstCard), mapper.Map<CardResult>(source.Result.Player.SecondCard) };
                        if (source.Result.Player.ThirdCard != null)
                        {
                            result.Add(mapper.Map<CardResult>(source.Result.Player.ThirdCard));
                        }

                        return result;
                    }

                    return Enumerable.Empty<CardResult>();
                }))
                .ForMember(x => x.BankerCards, opt => opt.MapFrom((source, destination, i, context) =>
                {
                    if (source.Result != null && source.Result.Banker != null)
                    {
                        var mapper = context.Mapper;
                        var result = new List<CardResult> { mapper.Map<CardResult>(source.Result.Banker.FirstCard), mapper.Map<CardResult>(source.Result.Banker.SecondCard) };
                        if (source.Result.Banker.ThirdCard != null)
                        {
                            result.Add(mapper.Map<CardResult>(source.Result.Banker.ThirdCard));
                        }

                        return result;
                    }

                    return Enumerable.Empty<CardResult>();
                }));
            CreateMap<SicBoBetReportEntity, SicBoBetRecord>();
            CreateMap<SicBoRoundEntity, SicBoRoundResult>()
                .ForMember(x => x.FirstDice, opt => opt.MapFrom((source) => source.Result.FirstDice))
                .ForMember(x => x.SecondDice, opt => opt.MapFrom((source) => source.Result.SecondDice))
                .ForMember(x => x.ThirdDice, opt => opt.MapFrom((source) => source.Result.ThirdDice));

            CreateMap<SicBoBetEntity, SicBoBetHistoryRecord>();


            CreateMap<VipSeDieWarRoundEntity, VipSeDieWarRoundResult>()
                .ForMember(x => x.PlayerChips, opt => opt.MapFrom((source) => new[]
                {
                    source.Result.Player.FirstChip,
                    source.Result.Player.SecondChip,
                    source.Result.Player.ThirdChip,
                    source.Result.Player.FourthChip
                }))
                .ForMember(x => x.BankerChips, opt => opt.MapFrom((source) => new[]
                {
                    source.Result.Banker.FirstChip,
                    source.Result.Banker.SecondChip,
                    source.Result.Banker.ThirdChip,
                    source.Result.Banker.FourthChip
                }));

            CreateMap<VipSeDieWarBetReportEntity, VipSeDieWarBetRecord>();
            CreateMap<VipSeDieWarBetEntity, VipSeDieWarBetHistoryRecord>();
        }
    }
}
