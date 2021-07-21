using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ClientSites
{
    public class ReportHandler :
        IRequestHandler<GetBigSmallReportRequest, BigSmallReportResultViewModel>,
        IRequestHandler<GetBigSmallTurboReportRequest, BigSmallTurboReportResultViewModel>,
        IRequestHandler<GetOddEvenReportRequest, OddEvenReportResultViewModel>,
        IRequestHandler<GetOddEvenTurboReportRequest, OddEvenTurboReportResultViewModel>,
        IRequestHandler<GetSeDieReportRequest, SeDieReportResultViewModel>,
        IRequestHandler<GetMiniSeDieReportRequest, SeDieReportResultViewModel>,
        IRequestHandler<GetBigSmallBetHistoryRequest, IEnumerable<BigSmallBetHistoryViewModel>>,
        IRequestHandler<GetBigSmallTurboBetHistoryRequest, IEnumerable<BigSmallBetHistoryViewModel>>,
        IRequestHandler<GetOddEvenBetHistoryRequest, IEnumerable<OddEvenBetHistoryViewModel>>,
        IRequestHandler<GetOddEvenTurboBetHistoryRequest, IEnumerable<OddEvenBetHistoryViewModel>>,
        IRequestHandler<GetKenoReportRequest, KenoReportViewModel>,
        IRequestHandler<GetKenoBetHistoryRequest, KenoBetHistoryViewModel>,
        IRequestHandler<GetFishPrawnCrabReportRequest, FishPrawnCrabReportResultViewModel>,
        IRequestHandler<GetDragonTigerReportRequest, DragonTigerReportResultViewModel>,
        IRequestHandler<GetBaccaratReportRequest, BaccaratReportResultViewModel>,
        IRequestHandler<GetVipBaccaratReportRequest, VipBaccaratReportResultViewModel>,
        IRequestHandler<GetRouletteReportRequest, RouletteReportResultViewModel>,
        IRequestHandler<GetRouletteBetHistoryRequest, RouletteBetHistoryViewModel>,
        IRequestHandler<GetLuckyWheelHistoryRequest, LuckyWheelHistoryViewModel>,
        IRequestHandler<GetVipSeDieReportRequest, VipSeDieReportResultViewModel>,
        IRequestHandler<GetSpecialSeDieReportRequest, SpecialSeDieReportResultViewModel>,
        IRequestHandler<GetSpecialSeDieBetHistoryRequest, SpecialSeDieBetHistoryViewModel>,
        IRequestHandler<GetBlackjackReportRequest, BlackjackReportResultViewModel>,
        IRequestHandler<GetVipFishPrawnCrabReportRequest, VipFishPrawnCrabReportResultViewModel>,
        IRequestHandler<GetSicBoReportRequest, SicBoReportResultViewModel>,
        IRequestHandler<GetSicBoBetHistoryRequest, SicBoBetHistoryViewModel>,
        IRequestHandler<GetVipSeDieWarReportRequest, VipSeDieWarReportResultViewModel>,
        IRequestHandler<GetVipSeDieWarBetHistoryRequest, VipSeDieWarBetHistoryViewModel>,
        IRequestHandler<GetSlotGameReportRequest, SlotGameReportResultViewModel>,
        IRequestHandler<GetJackpotReportRequest, IEnumerable<JackpotRecordViewModel>>,
        IRequestHandler<GetLotteryReportRequest, LotteryReportResultViewModel>,
        IRequestHandler<GetLotteryBetHistoryRequest, LotteryBetHistoryViewModel>,
        IRequestHandler<GetThreeCardsReportRequest, ThreeCardsReportResultViewModel>,
        IRequestHandler<GetKenoSoccerReportRequest, KenoSoccerReportResultViewModel>,
        IRequestHandler<GetKenoSoccerBetHistoryRequest, KenoSoccerBetHistoryViewModel>

    {
        private readonly IReportService reportService;
        private readonly IAppSettings appSettings;
        private readonly IMapper mapper;

        public ReportHandler(IReportService reportService, IAppSettings appSettings, IMapper mapper)
        {
            this.reportService = reportService;
            this.appSettings = appSettings;
            this.mapper = mapper;
        }

        public async Task<BigSmallReportResultViewModel> Handle(GetBigSmallReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBigSmallReport(request.RoundId, request.Nickname, request.ExcludeBot);

            return new BigSmallReportResultViewModel
            {
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                },
                Records = result.Records.Select(x => new BigSmallRecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Select = x.Select
                })
            };
        }

        public async Task<BigSmallTurboReportResultViewModel> Handle(GetBigSmallTurboReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBigSmallTurboReport(request.RoundId, request.Nickname, request.ExcludeBot);

            return new BigSmallTurboReportResultViewModel
            {
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                },
                Records = result.Records.Select(x => new BigSmallRecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Select = x.Select
                })
            };
        }

        public async Task<OddEvenReportResultViewModel> Handle(GetOddEvenReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetOddEvenReport(request.RoundId, request.Nickname, request.ExcludeBot);

            return new OddEvenReportResultViewModel
            {
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                },
                Records = result.Records.Select(x => new OddEvenRecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Select = x.Select
                })
            };
        }

        public async Task<OddEvenTurboReportResultViewModel> Handle(GetOddEvenTurboReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetOddEventTurboReport(request.RoundId, request.Nickname, request.ExcludeBot);

            return new OddEvenTurboReportResultViewModel
            {
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                },
                Records = result.Records.Select(x => new OddEvenRecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Select = x.Select
                })
            };
        }

        public async Task<IEnumerable<BigSmallBetHistoryViewModel>> Handle(GetBigSmallBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBigSmallBetHistory(request.RoundId, request.Nickname);

            return result.Select(x => new BigSmallBetHistoryViewModel
            {
                Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                BetWin = x.BetWin,
                Refund = x.Refund,
                Round = x.Round,
                BetResult = new BigSmallBetResultViewModel
                {
                    Dices = x.BetResult.Dices
                },
                Choice = new BigSmallBetChoiceViewModel
                {
                    Amount = x.Choice.Amount,
                    Big = x.Choice.Big
                }
            });
        }

        public async Task<IEnumerable<BigSmallBetHistoryViewModel>> Handle(GetBigSmallTurboBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBigSmallTurboBetHistory(request.RoundId, request.Nickname);

            return result.Select(x => new BigSmallBetHistoryViewModel
            {
                Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                BetWin = x.BetWin,
                Refund = x.Refund,
                Round = x.Round,
                BetResult = new BigSmallBetResultViewModel
                {
                    Dices = x.BetResult.Dices
                },
                Choice = new BigSmallBetChoiceViewModel
                {
                    Amount = x.Choice.Amount,
                    Big = x.Choice.Big
                }
            });
        }

        public async Task<IEnumerable<OddEvenBetHistoryViewModel>> Handle(GetOddEvenBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetOddEvenBetHistory(request.RoundId, request.Nickname);

            return result.Select(x => new OddEvenBetHistoryViewModel
            {
                Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                BetWin = x.BetWin,
                Refund = x.Refund,
                Round = x.Round,
                BetResult = new OddEvenBetResultViewModel
                {
                    Dices = x.BetResult.Dices
                },
                Choice = new OddEvenBetChoiceViewModel
                {
                    Amount = x.Choice.Amount,
                    Even = x.Choice.Even
                }
            });
        }

        public async Task<IEnumerable<OddEvenBetHistoryViewModel>> Handle(GetOddEvenTurboBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetOddEvenTurboBetHistory(request.RoundId, request.Nickname);

            return result.Select(x => new OddEvenBetHistoryViewModel
            {
                Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                BetWin = x.BetWin,
                Refund = x.Refund,
                Round = x.Round,
                BetResult = new OddEvenBetResultViewModel
                {
                    Dices = x.BetResult.Dices
                },
                Choice = new OddEvenBetChoiceViewModel
                {
                    Amount = x.Choice.Amount,
                    Even = x.Choice.Even
                }
            });
        }

        public async Task<SeDieReportResultViewModel> Handle(GetSeDieReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetSeDieReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new SeDieReportResultViewModel
            {
                RoundResult = mapper.Map<SeDieRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<SeDieRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<SeDieReportResultViewModel> Handle(GetMiniSeDieReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetMiniSeDieReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new SeDieReportResultViewModel
            {
                RoundResult = mapper.Map<SeDieRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<SeDieRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<KenoReportViewModel> Handle(GetKenoReportRequest request, CancellationToken cancellationToken)
        {
            var roundResult = await reportService.GetKenoRoundResult(request.Game, request.RoundId);
            var betRecords = await reportService.GetKenoBetReport(request.Game, request.RoundId, request.Nickname, request.ExcludeUUS);

            return new KenoReportViewModel(
                request.Game,
                request.RoundId,
                roundResult,
                betRecords,
                appSettings.DefaultTimeZone);
        }

        public async Task<KenoBetHistoryViewModel> Handle(GetKenoBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var betHistories = await reportService.GetKenoBetHistory(request.Game, request.RoundId, request.Nickname);

            return new KenoBetHistoryViewModel(betHistories, appSettings.DefaultTimeZone);
        }

        public async Task<FishPrawnCrabReportResultViewModel> Handle(GetFishPrawnCrabReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetFishPrawnCrabBetReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new FishPrawnCrabReportResultViewModel
            {
                RoundResult = mapper.Map<FishPrawnCrabRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<FishPrawnCrabRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<DragonTigerReportResultViewModel> Handle(GetDragonTigerReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetDragonTigerBetReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new DragonTigerReportResultViewModel
            {
                RoundResult = mapper.Map<DragonTigerRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<DragonTigerRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<BaccaratReportResultViewModel> Handle(GetBaccaratReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBaccaratBetReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new BaccaratReportResultViewModel
            {
                RoundResult = mapper.Map<BaccaratRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<BaccaratRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<RouletteReportResultViewModel> Handle(GetRouletteReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetRouletteBetReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new RouletteReportResultViewModel
            {
                RoundResult = mapper.Map<RouletteRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<RouletteRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<RouletteBetHistoryViewModel> Handle(GetRouletteBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetRouletteBetHistory(request.RoundId, request.UserId);

            return new RouletteBetHistoryViewModel
            {
                RoundResult = mapper.Map<RouletteRoundResultViewModel>(result.RoundResult),
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<RouletteBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                })
            };
        }

        public async Task<VipSeDieReportResultViewModel> Handle(GetVipSeDieReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetVipSeDieReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new VipSeDieReportResultViewModel
            {
                RoundResult = mapper.Map<SeDieRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x => mapper.Map<VipSeDieRecordViewModel>(x))
            };
        }

        public async Task<SpecialSeDieReportResultViewModel> Handle(GetSpecialSeDieReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetSpecialSeDieReport(request.RoundId, request.Nickname, request.ExcludeBot);

            return new SpecialSeDieReportResultViewModel
            {
                RoundResult = mapper.Map<SeDieRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<SpecialSeDieRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<SpecialSeDieBetHistoryViewModel> Handle(GetSpecialSeDieBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetSpecialSeDieBetHistory(request.RoundId, request.UserId);

            return new SpecialSeDieBetHistoryViewModel
            {
                RoundResult = mapper.Map<SeDieRoundResultViewModel>(result.RoundResult),
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<SpecialSeDieBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                })
            };
        }

        public async Task<VipFishPrawnCrabReportResultViewModel> Handle(GetVipFishPrawnCrabReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetVipFishPrawnCrabBetReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new VipFishPrawnCrabReportResultViewModel
            {
                RoundResult = mapper.Map<VipFishPrawnCrabRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<VipFishPrawnCrabRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<BlackjackReportResultViewModel> Handle(GetBlackjackReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBlackjackReport(request.HandId, request.Nickname, request.ExcludeUUS);

            return new BlackjackReportResultViewModel
            {
                Records = result.Select(x =>
                {
                    var betViewModel = mapper.Map<BlackjackRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                })
            };
        }

        public async Task<SlotGameReportResultViewModel> Handle(GetSlotGameReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetSlotGameReport(request.RoundId, request.Nickname, request.ExcludeUUS, request.Game);

            return new SlotGameReportResultViewModel
            {
                Records = result.Select(x =>
                {
                    var betViewModel = mapper.Map<SlotGameRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    betViewModel.Game = request.Game.Value;

                    return betViewModel;
                })
            };
        }

        public async Task<LuckyWheelHistoryViewModel> Handle(GetLuckyWheelHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetLuckyWheelBetHistory(request.Nickname);

            return new LuckyWheelHistoryViewModel(result, appSettings.DefaultTimeZone);
        }

        public async Task<SicBoReportResultViewModel> Handle(GetSicBoReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetSicBoBetReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new SicBoReportResultViewModel
            {
                RoundResult = mapper.Map<SicBoRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<SicBoRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<SicBoBetHistoryViewModel> Handle(GetSicBoBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetSicBoBetHistory(request.RoundId, request.UserId);

            return new SicBoBetHistoryViewModel
            {
                RoundResult = mapper.Map<SicBoRoundResultViewModel>(result.RoundResult),
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<SicBoBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                })
            };
        }

        public async Task<VipSeDieWarReportResultViewModel> Handle(GetVipSeDieWarReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetVipSeDieWarBetReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new VipSeDieWarReportResultViewModel
            {
                RoundResult = mapper.Map<VipSeDieWarRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<VipSeDieWarRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<VipSeDieWarBetHistoryViewModel> Handle(GetVipSeDieWarBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetVipSeDieWarBetHistory(request.RoundId, request.UserId);

            return new VipSeDieWarBetHistoryViewModel
            {
                RoundResult = mapper.Map<VipSeDieWarRoundResultViewModel>(result.RoundResult),
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<VipSeDieWarBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                })
            };
        }

        public async Task<VipBaccaratReportResultViewModel> Handle(GetVipBaccaratReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetVipBaccaratBetReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new VipBaccaratReportResultViewModel
            {
                RoundResult = mapper.Map<BaccaratRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<BaccaratRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<IEnumerable<JackpotRecordViewModel>> Handle(GetJackpotReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetJackpotReport(request.GameId, request.FromDate, request.ToDate, request.ExcludeUUS, request.ExcludeBot);

            return result.Select(x =>
            {
                var betViewModel = mapper.Map<JackpotRecordViewModel>(x);
                betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                return betViewModel;
            });
        }

        public async Task<LotteryReportResultViewModel> Handle(GetLotteryReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetLotteryBetReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new LotteryReportResultViewModel
            {
                RoundResult = mapper.Map<LotteryRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<LotteryRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<LotteryBetHistoryViewModel> Handle(GetLotteryBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetLotteryBetHistory(request.RoundId, request.UserId);

            return new LotteryBetHistoryViewModel
            {
                RoundResult = mapper.Map<LotteryRoundResultViewModel>(result.RoundResult),
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<LotteryBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                }),
                Odds = mapper.Map<LotteryOddsViewModel>(result.Odds),
            };
        }

        public async Task<ThreeCardsReportResultViewModel> Handle(GetThreeCardsReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetThreeCardsBetReport(request.RoundId, request.Nickname, request.ExcludeUUS);

            return new ThreeCardsReportResultViewModel
            {
                RoundResult = mapper.Map<ThreeCardsRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<ThreeCardsRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<KenoSoccerReportResultViewModel> Handle(GetKenoSoccerReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetKenoSoccerBetReport(request.Game, request.RoundId, request.Nickname, request.ExcludeUUS);

            return new KenoSoccerReportResultViewModel
            {
                Game = request.Game,
                RoundResult = mapper.Map<KenoSoccerRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<KenoSoccerRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return recordViewModel;
                })
            };
        }

        public async Task<KenoSoccerBetHistoryViewModel> Handle(GetKenoSoccerBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetKenoSoccerBetHistory(request.Game, request.RoundId, request.UserId);

            return new KenoSoccerBetHistoryViewModel
            {
                RoundResult = mapper.Map<KenoSoccerRoundResultViewModel>(result.RoundResult),
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<KenoSoccerBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                }),
            };
        }
    }
}