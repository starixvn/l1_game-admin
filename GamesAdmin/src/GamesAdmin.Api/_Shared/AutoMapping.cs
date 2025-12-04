using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Core.Models.Spins;
using GamesAdmin.Core.Models.Um;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.ShootingBlitz;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.DragonTigerFight;
using GamesAdmin.Database.Entities.Lobby;
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

            CreateMap<BaseBaccaratRoundEntity, BaccaratRoundResult>()
             .ForMember(x => x.PlayerCards, opt => opt.MapFrom((source, destination, i, context) =>
             {
                 return Enumerable.Empty<DragonTigerCardResult>();
             }))
             .ForMember(x => x.BankerCards, opt => opt.MapFrom((source, destination, i, context) =>
             {
                 return Enumerable.Empty<DragonTigerCardResult>();
             }));

            CreateMap<RouletteBetReportEntity, RouletteBetRecord>();
            CreateMap<RouletteRoundEntity, RouletteRoundResult>();
            CreateMap<RouletteSettlementResultEntity, RouletteSettlementResult>();
            CreateMap<RouletteBetEntity, RouletteBetHistoryRecord>();

            CreateMap<MiniRouletteBetReportEntity, RouletteBetRecord>();
            CreateMap<MiniRouletteRoundEntity, RouletteRoundResult>();
            CreateMap<MiniRouletteSettlementResultEntity, RouletteSettlementResult>();
            CreateMap<MiniRouletteBetEntity, RouletteBetHistoryRecord>();

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
               .ForMember(x => x.Result, opt => opt.MapFrom((source) => source.SettlementResult))
               .ForMember(x => x.PlayerCards, opt => opt.MapFrom((source, destination, i, context) =>
               {
                   if (source != null && source.Result.Player != null)
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
                   if (source != null && source.Result.Banker != null)
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
               .ForMember(x => x.PartnerId, opt => opt.MapFrom((source) => source.OperatorId))
               .ForMember(x => x.ExchangRateId, opt => opt.MapFrom((source) => source.ExchangeRateId));
            CreateMap<SlotGameSettlementResultEntity, SlotGameSettlementResult>();
            CreateMap<SlotGameFreeSpinEntity, SlotGameFreeSpin>();
            CreateMap<SlotGameBonusEntity, SlotGameBonus>();
            CreateMap<SlotGameWinningLineEntity, SlotGameWinningLine>();
            CreateMap<SlotGameBonusChestEntity, SlotGameBonusChest>();
            CreateMap<SlotGameJackpotEntity, SlotGameJackpot>();
            CreateMap<SlotJackpotContributionEntity, SlotJackpotContribution>();

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

            CreateMap<MiniSeDieMultiplayRoundEntity, SeDieRoundResult>()
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

                    if (source.Call > 0)
                    {
                        choiceHistories.Add(new ChoiceHistory("CALL", source.CallOdds, source.CallTime, source.Call, source.SettlementResult?.Call ?? 0));
                    }

                    if (source.Turn > 0)
                    {
                        choiceHistories.Add(new ChoiceHistory("TURN", source.TurnOdds, source.TurnTime, source.Turn, source.SettlementResult?.Turn ?? 0));
                    }

                    if (source.River > 0)
                    {
                        choiceHistories.Add(new ChoiceHistory("RIVER", source.RiverOdds, source.RiverTime, source.River, source.SettlementResult?.River ?? 0));
                    }

                    return choiceHistories;
                }

                return Enumerable.Empty<ChoiceHistory>();
            }));

            CreateMap<BigSmallSettlementResultEntity, BigSmallSettlementResult>();
            CreateMap<BigSmallRoundEntity, BigSmallRoundResult>()
                .ForMember(x => x.Dice1, opt => opt.MapFrom((source) => source.Result.Dice1))
                .ForMember(x => x.Dice2, opt => opt.MapFrom((source) => source.Result.Dice2))
                .ForMember(x => x.Dice3, opt => opt.MapFrom((source) => source.Result.Dice3));

            CreateMap<UnbalancedBigSmallBetReportEntity, UnbalancedBigSmallBetRecord>();
            CreateMap<UnbalancedBigSmallBetLogEntity, UnbalancedBigSmallBetHistoryRecord>();

            CreateMap<FruitSlotRoundEntity, SlotRNGRoundResult>()
                 .ForMember(x => x.ResultNumber, opt => opt.MapFrom((source) => source.Result.Number))
                 .ForMember(x => x.Symbols, opt => opt.MapFrom((source) => source.Result.Symbols));
            CreateMap<FruitSlotBetReportEntity, SlotRNGBetRecord>()
                .ForMember(x => x.BetWin, opt => opt.MapFrom((source) => source.Betwin))
                .ForMember(x => x.PartnerId, opt => opt.MapFrom((source) => source.OperatorId));
            CreateMap<FruitSlotBetLogEntity, SlotRNGBetHistoryRecord>();

            CreateMap<FanTanBetReportEntity, FanTanBetRecord>();
            CreateMap<FanTanRoundEntity, FanTanRoundResult>();
            CreateMap<FanTanSettlementResultEntity, FanTanSettlementResult>();
            CreateMap<FanTanResultEntity, FanTanResult>();
            CreateMap<FanTanBetEntity, FanTanBetHistoryRecord>();

            CreateMap<WinThreeCardsCardEntity, PokerCard>();
            CreateMap<WinThreeCardsHandResultEntity, WinThreeCardsHandResult>()
                .ForMember(x => x.Cards,
                opt => opt.MapFrom((source, destination, i, context) =>
                {
                    var mapper = context.Mapper;
                    return new List<PokerCard>() {
                            mapper.Map<PokerCard>(source.Cards.FirstCard),
                            mapper.Map<PokerCard>(source.Cards.SecondCard),
                            mapper.Map<PokerCard>(source.Cards.ThirdCard)
                        };
                }));
            CreateMap<WinThreeCardsResultEntity, WinThreeCardsResult>();
            CreateMap<WinThreeCardsRoundEntity, WinThreeCardsRoundResult>();
            CreateMap<WinThreeCardsBetEntity, WinThreeCardsBetRecord>();
            CreateMap<WinThreeCardsBetLogEntity, WinThreeCardsBetHistoryRecord>();
            CreateMap<WinThreeCardsBetReportEntity, WinThreeCardsBetRecord>()
                .ForMember(x => x.BetWin, opt => opt.MapFrom(source => source.BetWin))
                .ForMember(x => x.PartnerId, opt => opt.MapFrom(source => source.OperatorId));

            CreateMap<LobbyEntity, LobbyModel>()
             .ForMember(x => x.Id, opt => opt.MapFrom((source) => source.Id.ToString()))
             .ForMember(x => x.Categories, opt => opt.MapFrom((source, destination, i, context) =>
             {
                 var result = source.Categories.Select(category =>
                 {
                     return new LobbyCategory
                     {
                         Name = new CategoryName
                         {
                             EN_US = category.Name.EN_US,
                             ID_ID = category.Name.ID_ID,
                             TH_TH = category.Name.TH_TH,
                             VI_VN = category.Name.VI_VN,
                             ZH_CN = category.Name.ZH_CN,
                             ZH_TW = category.Name.ZH_TW,
                             JA_JP = category.Name.JA_JP,
                             ES_ES = category.Name.ES_ES
                         },
                         GameSettings = category.GameSettings.Select(gameSetting => new GameSetting { GameId = gameSetting.GameId, BigSize = gameSetting.BigSize, NewTag = gameSetting.NewTag }).ToArray(),
                     };
                 }).ToArray();

                 return result;
             }))
              .ForMember(x => x.RoundTimerGame, opt => opt.MapFrom((source) => new RoundTimerGame
              {
                  Games = source.RoundTimerGame.Games.Select(game => new Game { Id = game.GameId, Name = game.Name }).ToArray(),
                  Main = source.RoundTimerGame.Main
              }))
             .ForMember(x => x.Banners, opt => opt.MapFrom((source, destination, i, context) =>
             {
                 var result = source.Banners.Select(banner =>
                 {
                     return new Banner
                     {
                         GameId = banner.GameId,
                         DisplayTime = banner.DisplayTime
                     };
                 }).ToArray();

                 return result;
             }))
             .ForMember(x => x.CenterBanners, opt => opt.MapFrom((source, destination, i, context) =>
             {
                 if (source.CenterBanners == null)
                 {
                     return Enumerable.Empty<Banner>().ToArray();
                 }

                 var result = source.CenterBanners.Select(banner =>
                 {
                     return new Banner
                     {
                         GameId = banner.GameId,
                         DisplayTime = banner.DisplayTime
                     };
                 }).ToArray();

                 return result;
             }));

            CreateMap<LobbyModel, LobbyEntity>()
              .ForMember(x => x.Id, opt => opt.MapFrom((source) => ObjectId.Parse(source.Id)))
              .ForMember(x => x.RoundTimerGame, opt => opt.MapFrom((source) => new RoundTimerGameEntity
              {
                  Games = source.RoundTimerGame.Games.Select(game => new GameEnitity { GameId = game.Id, Name = game.Name }).ToArray(),
                  Main = source.RoundTimerGame.Main
              }))
              .ForMember(x => x.Categories, opt => opt.MapFrom((source, destination, i, context) =>
              {
                  var result = source.Categories.Select(category =>
                  {
                      return new LobbyCategoryEntity
                      {
                          Name = new CategoryNameEntity
                          {
                              EN_US = category.Name.EN_US,
                              ID_ID = category.Name.ID_ID,
                              TH_TH = category.Name.TH_TH,
                              VI_VN = category.Name.VI_VN,
                              ZH_CN = category.Name.ZH_CN,
                              ZH_TW = category.Name.ZH_TW,
                              JA_JP = category.Name.JA_JP,
                              ES_ES = category.Name.ES_ES
                          },
                          GameSettings = category.GameSettings.Select(gameSetting => new GameSettingEntity { GameId = gameSetting.GameId, BigSize = gameSetting.BigSize, NewTag = gameSetting.NewTag }).ToArray()
                      };
                  }).ToArray();

                  return result;
              }))
              .ForMember(x => x.Banners, opt => opt.MapFrom((source, destination, i, context) =>
              {
                  var result = source.Banners.Select(banner =>
                  {
                      return new BannerEntity
                      {
                          GameId = banner.GameId,
                          DisplayTime = banner.DisplayTime
                      };
                  }).ToArray();

                  return result;
              }))
              .ForMember(x => x.CenterBanners, opt => opt.MapFrom((source, destination, i, context) =>
              {
                  if (source.CenterBanners == null)
                  {
                      return Enumerable.Empty<BannerEntity>().ToArray();
                  }

                  var result = source.CenterBanners.Select(banner =>
                  {
                      return new BannerEntity
                      {
                          GameId = banner.GameId,
                          DisplayTime = banner.DisplayTime
                      };
                  }).ToArray();

                  return result;
              }))
              ;
            //CreateMap<Database.Entities.Lobby.LobbyCategory, Core.Models.Lobby.LobbyCategory>();
            CreateMap<DirectLobbyEntity, DirectLobbyModel>()
             .ForMember(x => x.Id, opt => opt.MapFrom((source) => source.Id.ToString()));
            CreateMap<DirectLobbyModel, DirectLobbyEntity>()
              .ForMember(x => x.Id, opt => opt.MapFrom((source) => ObjectId.Parse(source.Id)));


            CreateMap<DragonTigerFightRoundEntity, DragonTigerFightRoundResult>()
                .ForMember(dest => dest.Dragon, opt => opt.MapFrom(src => src.Result.Dragon))
                .ForMember(dest => dest.Tiger, opt => opt.MapFrom(src => src.Result.Tiger))
                .ForMember(dest => dest.SettlementResult, opt => opt.MapFrom(src => src.SettlementResult));
            CreateMap<DragonTigerFightBetReportEntity, DragonTigerFightBetRecord>()
                .ForMember(dest => dest.PartnerId, opt => opt.MapFrom(src => src.OperatorId));
            CreateMap<DragonTigerFightBetLogEntity, DragonTigerFightBetHistoryRecord>();

            CreateMap<ColorGameRoundEntity, ColorGameRoundResult>()
                .ForMember(dest => dest.Dice1, opt => opt.MapFrom(src => src.Result.Dice1))
                .ForMember(dest => dest.Dice2, opt => opt.MapFrom(src => src.Result.Dice2))
                .ForMember(dest => dest.Dice3, opt => opt.MapFrom(src => src.Result.Dice3))
                .ForMember(dest => dest.SettlementResult, opt => opt.MapFrom(src => src.SettlementResult));
            CreateMap<ColorGameBetReportEntity, ColorGameBetRecord>()
                .ForMember(dest => dest.PartnerId, opt => opt.MapFrom(src => src.OperatorId));
            CreateMap<ColorGameBetLogEntity, ColorGameBetHistoryRecord>();

            CreateMap<FootballDiceBetReportEntity, FootballDiceBetRecord>()
                .ForMember(dest => dest.PartnerId, opt => opt.MapFrom(src => src.OperatorId));
            CreateMap<FootballDiceRoundEntity, FootballDiceRoundResult>();
            CreateMap<FootballDiceSettlementResultEntity, FootballDiceSettlementResult>();
            CreateMap<FootballDiceResultEntity, FootballDiceResult>();
            CreateMap<FootballDiceBetLogEntity, FootballDiceBetHistoryRecord>();

            CreateMap<BandarQiuQiuRoundEntity, BandarQiuQiuRoundResult>()
                .ForMember(dest => dest.Banker, opt => opt.MapFrom(src => new BandarQiuQiuResultItem()
                {
                    Dominoes = src.Result.Banker.Dominoes,
                    HandRankingId = src.Result.Banker.HandRankingId,
                    PipsOfSet = src.Result.Banker.PipsOfSet
                }))
                .ForMember(dest => dest.Spade, opt => opt.MapFrom(src => new BandarQiuQiuResultItem()
                {
                    Dominoes = src.Result.Spade.Dominoes,
                    HandRankingId = src.Result.Spade.HandRankingId,
                    PipsOfSet = src.Result.Spade.PipsOfSet
                }))
                .ForMember(dest => dest.Heart, opt => opt.MapFrom(src => new BandarQiuQiuResultItem()
                {
                    Dominoes = src.Result.Heart.Dominoes,
                    HandRankingId = src.Result.Heart.HandRankingId,
                    PipsOfSet = src.Result.Heart.PipsOfSet
                }))
                .ForMember(dest => dest.Club, opt => opt.MapFrom(src => new BandarQiuQiuResultItem()
                {
                    Dominoes = src.Result.Club.Dominoes,
                    HandRankingId = src.Result.Club.HandRankingId,
                    PipsOfSet = src.Result.Club.PipsOfSet
                }))
                .ForMember(dest => dest.Diamond, opt => opt.MapFrom(src => new BandarQiuQiuResultItem()
                {
                    Dominoes = src.Result.Diamond.Dominoes,
                    HandRankingId = src.Result.Diamond.HandRankingId,
                    PipsOfSet = src.Result.Diamond.PipsOfSet
                }))
                .ForMember(dest => dest.SettlementResult, opt => opt.MapFrom(src => src.SettlementResult));
            CreateMap<BandarQiuQiuBetEntity, BandarQiuQiuBetRecord>()
                .ForMember(dest => dest.PartnerId, opt => opt.MapFrom(src => src.OperatorId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Uid));
            CreateMap<BandarQiuQiuBetLogEntity, BandarQiuQiuBetHistoryRecord>();

            CreateMap<CrashGameRoundEntity, CrashGameRoundResult>();
            CreateMap<CrashGameBetLogEntity, CrashGameBetHistory>();
            CreateMap<CrashGameBetReportEntity, CrashGameBetRecord>()
               .ForMember(x => x.RoundId, opt => opt.MapFrom((source) => source.Round))
               .ForMember(x => x.PartnerId, opt => opt.MapFrom((source) => source.PartnerId))
               .ForMember(x => x.ExchangeRateId, opt => opt.MapFrom((source) => source.ExchangeRateId));

            CreateMap<BacBall1RoundEntity, BacBall1Round>();
            CreateMap<BacBall1BetEntity, BacBall1BetRecord>()
                .ForMember(dest => dest.PartnerId, opt => opt.MapFrom(src => src.OperatorId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Uid));
            CreateMap<BacBall1BetLogEntity, BacBall1BetHistoryRecord>();

            CreateMap<BacBall2RoundEntity, BacBall2Round>();
            CreateMap<BacBall2BetEntity, BacBall2BetRecord>()
                .ForMember(dest => dest.PartnerId, opt => opt.MapFrom(src => src.OperatorId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Uid));
            CreateMap<BacBall2BetLogEntity, BacBall2BetHistoryRecord>();

            CreateMap<LuckyNext1RoundEntity, NextBallRound>();
            CreateMap<LuckyNext1BetEntity, NextBallBetRecord>()
                .ForMember(dest => dest.PartnerId, opt => opt.MapFrom(src => src.OperatorId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Uid));
            CreateMap<LuckyNext1BetLogEntity, NextBallBetHistoryRecord>();

            CreateMap<FootballSlotRoundEntity, SlotRNGRoundResult>()
                 .ForMember(x => x.ResultNumber, opt => opt.MapFrom((source) => source.Result.Number))
                 .ForMember(x => x.Symbols, opt => opt.MapFrom((source) => source.Result.Symbols));
            CreateMap<FootballSlotBetReportEntity, SlotRNGBetRecord>()
                .ForMember(x => x.BetWin, opt => opt.MapFrom((source) => source.Betwin))
                .ForMember(x => x.PartnerId, opt => opt.MapFrom((source) => source.OperatorId));
            CreateMap<FootballSlotBetLogEntity, SlotRNGBetHistoryRecord>();

            // ShootingBlitz mappings
            CreateMap<ShootingBlitzRtpConfigEntity, ShootingBlitzRtpConfig>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ReverseMap();
            CreateMap<MultiplierStepsConfigEntity, MultiplierStepsConfig>().ReverseMap();
            CreateMap<StarPlayerSettingsEntity, StarPlayerSettings>().ReverseMap();

            CreateMap<ShootingBlitzTeamEntity, ShootingBlitzTeam>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ReverseMap();
            CreateMap<ShootingBlitzPlayerEntity, ShootingBlitzPlayer>().ReverseMap();
        }
    }
}