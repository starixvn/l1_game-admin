using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Core.Models.Spins;
using GamesAdmin.Core.Models.Um;
using GamesAdmin.Database;
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

            CreateMap<GameConfigEntity, GameConfig>().ReverseMap();

            CreateMap<BigSmallRoundEntity, Round>();
            CreateMap<BigSmallOnesEntity, BetInfo>();

            CreateMap<User, AdminUserEntity>();
            CreateMap<UmSchedule, EventScheduleEntity>()
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start.UtcDateTime))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End.UtcDateTime))
                .ReverseMap();

            CreateMap<UmInfo, UMInfoEntity>()
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start.UtcDateTime))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End.UtcDateTime))
                .ReverseMap();


            CreateMap<FishPrawnCrabRoundEntity, FishPrawnCrabRoundResult>()
                .ForMember(dest => dest.FirstDice, opt => opt.MapFrom(src => src.FirstDice ?? src.Result.FirstDice))
                .ForMember(dest => dest.SecondDice, opt => opt.MapFrom(src => src.SecondDice ?? src.Result.SecondDice))
                .ForMember(dest => dest.ThirdDice, opt => opt.MapFrom(src => src.ThirdDice ?? src.Result.ThirdDice));

            CreateMap<FishPrawnCrabBetReportEntity, FishPrawnCrabBetRecord>();

            CreateMap<Database.Entities.Card, DragonTigerCardResult>();
            CreateMap<DragonTigerRoundEntity, DragonTigerRoundResult>()
                .ForMember(dest => dest.Dragon, opt => opt.MapFrom(src => src.Dragon ?? src.Result.Dragon))
                .ForMember(dest => dest.Tiger, opt => opt.MapFrom(src => src.Tiger ?? src.Result.Tiger));
            CreateMap<DragonTigerBetReportEntity, DragonTigerBetRecord>();

            CreateMap<BaccaratSettlementResultEntity, BaccaratSettlementResult>();
            CreateMap<BaccaratBetReportEntity, BaccaratBetRecord>();
            CreateMap<BaccaratRoundEntity, BaccaratRoundResult>()
              .ForMember(x => x.PlayerCards, opt => opt.MapFrom((source, destination, i, context) =>
              {
                  if (source.Player != null)
                  {
                      var mapper = context.Mapper;
                      var result = new List<DragonTigerCardResult> { mapper.Map<DragonTigerCardResult>(source.Player.FirstCard), mapper.Map<DragonTigerCardResult>(source.Player.SecondCard) };
                      if (source.Player.ThirdCard != null)
                      {
                          result.Add(mapper.Map<DragonTigerCardResult>(source.Player.ThirdCard));
                      }

                      return result;
                  }

                  return Enumerable.Empty<DragonTigerCardResult>();
              }))
              .ForMember(x => x.BankerCards, opt => opt.MapFrom((source, destination, i, context) =>
              {
                  if (source.Banker != null)
                  {
                      var mapper = context.Mapper;
                      var result = new List<DragonTigerCardResult> { mapper.Map<DragonTigerCardResult>(source.Banker.FirstCard), mapper.Map<DragonTigerCardResult>(source.Banker.SecondCard) };
                      if (source.Banker.ThirdCard != null)
                      {
                          result.Add(mapper.Map<DragonTigerCardResult>(source.Banker.ThirdCard));
                      }

                      return result;
                  }

                  return Enumerable.Empty<DragonTigerCardResult>();
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
            CreateMap<StakeBalancedBetReportEntity, SpecialSeDieBetRecord>()
                 .ForMember(x => x.Choice, opt => opt.MapFrom((source) => source.Choice))
                 .ForMember(x => x.UserId, opt => opt.MapFrom((source) => source.UserId))
                 .ForMember(x => x.Amount, opt => opt.MapFrom((source) => source.Bet))
                 .ForMember(x => x.Refund, opt => opt.MapFrom((source) => source.Back));


            CreateMap<SpecialSeDieBetEntity, SpecialSeDieBetHistoryRecord>()
                 .ForMember(x => x.Amount, opt => opt.MapFrom((source) => source.BaseAmount))
                 .ForMember(x => x.Refund, opt => opt.MapFrom((source) => source.BaseRefund))
                 .ForMember(x => x.BetWin, opt => opt.MapFrom((source) => source.BaseBetWin));

            CreateMap<BlackjackBetReportEntity, BlackjackBetRecord>();
            CreateMap<CardEntity, Core.Models.Card>();
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
                        var result = new List<DragonTigerCardResult>
                        {
                            mapper.Map<DragonTigerCardResult>(source.Result.Player.FirstCard), mapper.Map<DragonTigerCardResult>(source.Result.Player.SecondCard)
                        };

                        if (source.Result.Player.ThirdCard != null)
                        {
                            result.Add(mapper.Map<DragonTigerCardResult>(source.Result.Player.ThirdCard));
                        }

                        return result;
                    }

                    return Enumerable.Empty<DragonTigerCardResult>();
                }))
                .ForMember(x => x.BankerCards, opt => opt.MapFrom((source, destination, i, context) =>
                {
                    if (source.Result != null && source.Result.Banker != null)
                    {
                        var mapper = context.Mapper;
                        var result = new List<DragonTigerCardResult>
                        {
                            mapper.Map<DragonTigerCardResult>(source.Result.Banker.FirstCard), mapper.Map<DragonTigerCardResult>(source.Result.Banker.SecondCard)
                        };

                        if (source.Result.Banker.ThirdCard != null)
                        {
                            result.Add(mapper.Map<DragonTigerCardResult>(source.Result.Banker.ThirdCard));
                        }

                        return result;
                    }

                    return Enumerable.Empty<DragonTigerCardResult>();
                }));

            CreateMap<BlockchainBaccaratCard, DragonTigerCardResult>();

            CreateMap<BlockchainBaccaratRoundEntity, BaccaratRoundResult>()
               .ForMember(x => x.Result, opt => opt.MapFrom((source) => source.Result))
               .ForMember(x => x.PlayerCards, opt => opt.MapFrom((source, destination, i, context) =>
               {
                   if (source != null && source.Player != null)
                   {
                       var mapper = context.Mapper;
                       var result = new List<DragonTigerCardResult>
                       {
                            mapper.Map<DragonTigerCardResult>(source.Player.FirstCard), mapper.Map<DragonTigerCardResult>(source.Player.SecondCard)
                       };

                       if (source.Player.ThirdCard != null)
                       {
                           result.Add(mapper.Map<DragonTigerCardResult>(source.Player.ThirdCard));
                       }

                       return result;
                   }

                   return Enumerable.Empty<DragonTigerCardResult>();
               }))
               .ForMember(x => x.BankerCards, opt => opt.MapFrom((source, destination, i, context) =>
               {
                   if (source != null && source.Banker != null)
                   {
                       var mapper = context.Mapper;
                       var result = new List<DragonTigerCardResult>
                       {
                            mapper.Map<DragonTigerCardResult>(source.Banker.FirstCard), mapper.Map<DragonTigerCardResult>(source.Banker.SecondCard)
                       };

                       if (source.Banker.ThirdCard != null)
                       {
                           result.Add(mapper.Map<DragonTigerCardResult>(source.Banker.ThirdCard));
                       }

                       return result;
                   }

                   return Enumerable.Empty<DragonTigerCardResult>();
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

            CreateMap<SlotGameBetReportEntity, SlotGameBetRecord>()
               .ForMember(x => x.RoundId, opt => opt.MapFrom((source) => source.Number))
               .ForMember(x => x.Partner, opt => opt.MapFrom((source) => source.PartnerId))
               .ForMember(x => x.ExchangRateId, opt => opt.MapFrom((source) => source.ExchangeRateId));
            CreateMap<SlotGameSettlementResultEntity, SlotGameSettlementResult>();
            CreateMap<SlotGameFreeSpinEntity, SlotGameFreeSpin>();
            CreateMap<SlotGameBonusEntity, SlotGameBonus>();
            CreateMap<SlotGameWinningLineEntity, SlotGameWinningLine>();
            CreateMap<SlotGameBonusChestEntity, SlotGameBonusChest>();
            CreateMap<SlotGameJackpotEntity, SlotGameJackpot>();

            CreateMap<CascadingSlotBetReportEntity, CascadingSlotBetRecord>()
                .ForMember(x => x.RoundId, opt => opt.MapFrom((source) => source.Number))
                .ForMember(x => x.PartnerId, opt => opt.MapFrom((source) => source.OperatorId));
            CreateMap<CascadingSlotSettlementResultEntity, CascadingSlotSettlementResult>();
            CreateMap<CascadingSlotRoundMultiplierEntity, CascadingSlotRoundMultiplier>();
            CreateMap<CascadingSlotWinningSymbolEntity, CascadingSlotWinningSymbol>();
            CreateMap<CascadingSlotTumbleEntity, CascadingSlotTumble>();

            CreateMap<JackpotReportEntity, JackpotRecord>();

            CreateMap<SeDieRoundEntity, SeDieRoundResult>()
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

            CreateMap<MiniSeDieRoundEntity, SeDieRoundResult>()
               .ForMember(x => x.Chips, opt => opt.MapFrom((source) => new[]
               {
                   source.FirstRedChip ?? source.Result.FirstRedChip,
                   source.SecondRedChip ?? source.Result.SecondRedChip,
                   source.ThirdRedChip ?? source.Result.ThirdRedChip,
                   source.FourthRedChip ?? source.Result.FourthRedChip
               }))
               .ForMember(x => x.Odd, opt => opt.MapFrom((source) => source.SettlementResult.Odd))
               .ForMember(x => x.Even, opt => opt.MapFrom((source) => source.SettlementResult.Even))
               .ForMember(x => x.ThreeRed, opt => opt.MapFrom((source) => source.SettlementResult.ThreeRed))
               .ForMember(x => x.FourRed, opt => opt.MapFrom((source) => source.SettlementResult.FourRed))
               .ForMember(x => x.ThreeWhite, opt => opt.MapFrom((source) => source.SettlementResult.ThreeWhite))
               .ForMember(x => x.FourWhite, opt => opt.MapFrom((source) => source.SettlementResult.FourWhite));

            CreateMap<SeDieBetReportEntity, SeDieBetRecord>();

            CreateMap<LotteryRoundEntity, LotteryRoundResult>()
                .ForMember(x => x.Special, opt => opt.MapFrom((source) => source.Result.Special))
                .ForMember(x => x.First, opt => opt.MapFrom((source) => source.Result.First))
                .ForMember(x => x.Second, opt => opt.MapFrom((source) => source.Result.Second))
                .ForMember(x => x.Third, opt => opt.MapFrom((source) => source.Result.Third))
                .ForMember(x => x.Fourth, opt => opt.MapFrom((source) => source.Result.Fourth))
                .ForMember(x => x.Fifth, opt => opt.MapFrom((source) => source.Result.Fifth))
                .ForMember(x => x.Sixth, opt => opt.MapFrom((source) => source.Result.Sixth))
                .ForMember(x => x.Seventh, opt => opt.MapFrom((source) => source.Result.Seventh));

            CreateMap<LotterySettlementResultEntity, LotterySettlementResult>();
            CreateMap<LotteryOccurrencesEntity, LotteryOccurrences>();
            CreateMap<LotteryOthersSettlementResultEntity, LotteryOthersSettlementResult>();

            CreateMap<LotteryBetReportEntity, LotteryBetRecord>();

            CreateMap<LotteryBetEntity, LotteryBetHistoryRecord>();
            CreateMap<LotteryOddsEntity, LotteryOdds>();
            CreateMap<LotteryChoicesEntity, LotteryChoices>();

            CreateMap<ThreeCardsRoundEntity, ThreeCardsRoundResult>();
            CreateMap<ThreeCardsSettlementResultEntity, ThreeCardsSettlementResult>();
            CreateMap<ThreeCardsBetReportEntity, ThreeCardsBetRecord>();

            CreateMap<BitcoinMasterRoundResultEntity, BitcoinMasterRoundResult>();
            CreateMap<BitcoinMasterBetEntity, BitcoinMasterBetHistoryRecord>();

            CreateMap<KenoMatchRoundEntity, KenoMatchRoundResult>();
            CreateMap<KenoMatchPeriodEntity, KenoMatchPeriod>();
            CreateMap<KenoMatchPeriodResultEntity, KenoMatchPeriodResult>();
            CreateMap<KenoMatchPeriodSettlementResultEntity, KenoMatchPeriodSettlementResult>();
            CreateMap<KenoMatchBetReportEntity, KenoMatchBetRecord>();
            CreateMap<KenoMatchBetEntity, KenoMatchBetHistoryRecord>();

            CreateMap<KenoPowerRoundEntity, KenoPowerRoundResult>();
            CreateMap<KenoPowerResultEntity, KenoPowerResult>();
            CreateMap<KenoPowerSettlementResultEntity, KenoPowerSettlementResult>();
            CreateMap<KenoPowerBetReportEntity, KenoPowerBetRecord>();
            CreateMap<KenoPowerBetEntity, KenoPowerBetHistoryRecord>();

            CreateMap<CasinoHoldemPokerBetReportEntity, HoldemPokerBetRecord>()
                 .ForMember(x => x.PlayerResult, opt => opt.MapFrom((source) => (source.IsFold ? "Fold" : source.Player.Rank)))
                 .ForMember(x => x.DealerResult, opt => opt.MapFrom((source) => source.Ended && !source.IsFold ? (source.IsQualified.HasValue && !source.IsQualified.Value ? "Does Not Qualify" : source.Dealer.Rank) : "-"));

            CreateMap<CasinoHoldemPokerBetHistoryEntity, HoldemPokerBetHistory>()
            .ForMember(x => x.ChoiceHistories, opt => opt.MapFrom((source, destination, i, context) =>
            {
                if (source.Ante > 0)
                {
                    var choiceHistories = new List<ChoiceHistory>
                    {
                        new ChoiceHistory("ANTE", source.AnteOdds, source.Time, source.Ante, source.SettlementResult?.Ante ?? 0)
                    };

                    if (source.AA > 0)
                    {
                        choiceHistories.Add(new ChoiceHistory("AA", source.AaOdds, source.Time, source.AA, source.SettlementResult?.Aa ?? 0));
                    }

                    if (source.Ended && !source.IsFold)
                    {
                        choiceHistories.Add(new ChoiceHistory("CALL", source.CallOdds, source.CallTime, source.Call, source.SettlementResult?.Call ?? 0));
                    }

                    return choiceHistories;
                }


                return Enumerable.Empty<ChoiceHistory>();
            }));

            CreateMap<HiLoBetReportEntity, HiLoBetRecord>();
            CreateMap<HiLoBetLogEntity, HiLoBetHistoryRecord>();
            CreateMap<HiLoRoundEntity, HiLoRoundResult>();

            CreateMap<TexasHoldemPokerBetReportEntity, HoldemPokerBetRecord>()
                .ForMember(x => x.PlayerResult, opt => opt.MapFrom((source) => (source.IsFold ? "Fold" : source.Player.Rank)))
                .ForMember(x => x.DealerResult, opt => opt.MapFrom((source) => (source.Ended && !source.IsFold ? source.Dealer.Rank : "-")));


            CreateMap<TexasHoldemPokerBetHistoryEntity, HoldemPokerBetHistory>()
            .ForMember(x => x.ChoiceHistories, opt => opt.MapFrom((source, destination, i, context) =>
            {
                if (source.Ante > 0)
                {
                    var choiceHistories = new List<ChoiceHistory>
                    {
                        new ChoiceHistory("ANTE", source.AnteOdds, source.Time, source.Ante, source.SettlementResult?.Ante ?? 0)
                    };

                    if (source.Ended && !source.IsFold)
                    {
                        choiceHistories.Add(new ChoiceHistory("CALL", source.CallOdds, source.CallTime, source.Call, source.SettlementResult?.Call ?? 0));
                        choiceHistories.Add(new ChoiceHistory("TURN", source.TurnOdds, source.TurnTime, source.Turn, source.SettlementResult?.Turn ?? 0));
                        choiceHistories.Add(new ChoiceHistory("RIVER", source.RiverOdds, source.RiverTime, source.River, source.SettlementResult?.River ?? 0));
                    }

                    return choiceHistories;
                }


                return Enumerable.Empty<ChoiceHistory>();
            }));
        }
    }
}
