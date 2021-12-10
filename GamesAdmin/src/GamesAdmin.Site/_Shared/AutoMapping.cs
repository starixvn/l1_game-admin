using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Core.Models.Um;
using GamesAdmin.Site.Features.Dashboard.ViewModels;
using GamesAdmin.Site.Features.Notifications.Requests;
using GamesAdmin.Site.Features.Partners.ViewModels;
using GamesAdmin.Site.Features.Report.ViewModels;
using GamesAdmin.Site.Features.UmSchedule.ViewModels;

namespace GamesAdmin.Site._Shared
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<OnlineUser, OnlineUserViewModel>();
            CreateMap<BotUser, BotUserViewModel>();
            CreateMap<UmSchedule, UmScheduleItemViewModel>();
            CreateMap<Partner, PartnerEditViewModel>();

            CreateMap<AddRequest, Notification>();
            CreateMap<UpdateRequest, Notification>();
            CreateMap<FishPrawnCrabRoundResult, FishPrawnCrabRoundResultViewModel>();
            CreateMap<FishPrawnCrabBetRecord, FishPrawnCrabRecordViewModel>();

            CreateMap<DragonTigerRoundResult, DragonTigerRoundResultViewModel>();
            CreateMap<DragonTigerBetRecord, DragonTigerRecordViewModel>();
            CreateMap<DragonTigerCardResult, DragonTigerCardViewModel>();

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
            CreateMap<Card, CardViewModel>();

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

            CreateMap<SlotGameBetRecord, SlotGameRecordViewModel>();
            CreateMap<SlotGameSettlementResult, SlotGameSettlementResultViewModel>();
            CreateMap<SlotGameFreeSpin, SlotGameFreeSpinViewModel>();
            CreateMap<SlotGameBonus, SlotGameBonusViewModel>();
            CreateMap<SlotGameWinningLine, SlotGameWinningLineViewModel>();
            CreateMap<SlotGameBonusChest, SlotGameBonusChestViewModel>();
            CreateMap<SlotGameJackpot, SlotGameJackpotViewModel>();

            CreateMap<JackpotRecord, JackpotRecordViewModel>();

            CreateMap<LotteryRoundResult, LotteryRoundResultViewModel>();
            CreateMap<LotterySettlementResult, LotterySettlementResultViewModel>();
            CreateMap<LotteryOthersSettlementResult, LotteryOthersSettlementResultViewModel>();
            CreateMap<LotteryOccurrences, LotteryOccurrencesViewModel>();
            CreateMap<LotteryBetRecord, LotteryRecordViewModel>();

            CreateMap<LotteryBetHistoryRecord, LotteryBetHistoryRecordViewModel>();
            CreateMap<LotteryOdds, LotteryOddsViewModel>();
            CreateMap<LotteryChoices, LotteryChoicesViewModel>();

            CreateMap<ThreeCardsRoundResult, ThreeCardsRoundResultViewModel>();
            CreateMap<ThreeCardsSettlementResult, ThreeCardsSettlementResultViewModel>();
            CreateMap<ThreeCardsBetRecord, ThreeCardsRecordViewModel>();

            CreateMap<ThreeCardsRoundResult, ThreeCardsRoundResultViewModel>();
            CreateMap<ThreeCardsSettlementResult, ThreeCardsSettlementResultViewModel>();
            CreateMap<ThreeCardsBetRecord, ThreeCardsRecordViewModel>();
            
            CreateMap<KenoMatchRoundResult, KenoMatchRoundResultViewModel>();
            CreateMap<KenoMatchPeriod, KenoMatchPeriodViewModel>();
            CreateMap<KenoMatchPeriodResult, KenoMatchPeriodResultViewModel>();
            CreateMap<KenoMatchPeriodSettlementResult, KenoMatchPeriodSettlementResultViewModel>();
            CreateMap<KenoMatchBetRecord, KenoMatchRecordViewModel>();
            CreateMap<KenoMatchBetHistoryRecord, KenoMatchBetHistoryRecordViewModel>();

            CreateMap<KenoPowerRoundResult, KenoPowerRoundResultViewModel>();
            CreateMap<KenoPowerResult, KenoPowerResultViewModel>();
            CreateMap<KenoPowerSettlementResult, KenoPowerSettlementResultViewModel>();
            CreateMap<KenoPowerBetRecord, KenoPowerRecordViewModel>();
            CreateMap<KenoPowerBetHistoryRecord, KenoPowerBetHistoryRecordViewModel>();
        }
    }
}