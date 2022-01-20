using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Currencies;
using GamesAdmin.Site.Features.ExchangeRates;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Report
{
    public class ReportHandler :
        IRequestHandler<GetBigSmallReportRequest, BigSmallReportResultViewModel>,
        IRequestHandler<GetBigSmallTurboReportRequest, BigSmallTurboReportResultViewModel>,
        IRequestHandler<GetOddEvenReportRequest, OddEvenReportResultViewModel>,
        IRequestHandler<GetOddEvenTurboReportRequest, OddEvenTurboReportResultViewModel>,
        IRequestHandler<GetBigSmallBetHistoryRequest, IEnumerable<BigSmallBetHistoryViewModel>>,
        IRequestHandler<GetBigSmallTurboBetHistoryRequest, IEnumerable<BigSmallBetHistoryViewModel>>,
        IRequestHandler<GetOddEvenBetHistoryRequest, IEnumerable<OddEvenBetHistoryViewModel>>,
        IRequestHandler<GetOddEvenTurboBetHistoryRequest, IEnumerable<OddEvenBetHistoryViewModel>>,
        IRequestHandler<GetLuckyWheelHistoryRequest, LuckyWheelHistoryViewModel>,
        IRequestHandler<GetSpecialSeDieReportRequest, SpecialSeDieReportResultViewModel>,
        IRequestHandler<GetSpecialSeDieBetHistoryRequest, SpecialSeDieBetHistoryViewModel>,
        IRequestHandler<GetBlackjackReportRequest, BlackjackReportResultViewModel>,
        IRequestHandler<GetSlotGameReportRequest, SlotGameReportResultViewModel>,
        IRequestHandler<GetJackpotReportRequest, IEnumerable<JackpotRecordViewModel>>,
        IRequestHandler<GetBitcoinMasterReportRequest, BitcoinMasterReportResultViewModel>,
        IRequestHandler<GetBitcoinMasterBetHistoryRequest, BitcoinMasterBetHistoryViewModel>,
        IRequestHandler<GetCasinoHoldemPokerReportRequest, CasinoHoldemPokerReportResultViewModel>,
        IRequestHandler<GetSelectionsRequest, ReportSeletionsViewModel>,
        IRequestHandler<ReportRequest, ReportResultViewModel>,
        IRequestHandler<BetHistoryReportRequest, BetHistoryViewModel>,
        IRequestHandler<GetCasinoHoldemPokerBetHistoryRequest, CasinoHoldemPokerBetHistoryViewModel>
    {
        private readonly IReportService reportService;
        private readonly IAppSettings appSettings;
        private readonly IMapper mapper;
        private readonly IPartnerService partnerService;
        private readonly ICurrencyService currencyService;
        private readonly IExchangeRateService exchangeRateService;
        private readonly IReportServiceFactory reportServiceFactory;

        public ReportHandler(
            IReportService reportService,
            IAppSettings appSettings,
            IMapper mapper,
            IPartnerService partnerService,
            ICurrencyService currencyService,
            IExchangeRateService exchangeRateService,
            IReportServiceFactory reportServiceFactory)
        {
            this.reportService = reportService;
            this.appSettings = appSettings;
            this.mapper = mapper;
            this.partnerService = partnerService;
            this.currencyService = currencyService;
            this.exchangeRateService = exchangeRateService;
            this.reportServiceFactory = reportServiceFactory;
        }

        public async Task<BigSmallReportResultViewModel> Handle(GetBigSmallReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBigSmallReport(
                request.RoundId,
                request.Nickname,
                request.ExcludeBot,
                request.PartnerId,
                request.Username);
            var partners = await partnerService.GetPartnersForSelection(model => true);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, request.Currency);

            return new BigSmallReportResultViewModel
            {
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                    ExchangeRate = exchangeRate,
                },
                Records = result.Records.Select(x => new BigSmallRecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Select = x.Select,
                    Currency = x.Currency,
                    Partner = GetPartnerName(partners, x.PartnerId),
                    Rate = x.Rate,
                })
            };
        }

        public async Task<BigSmallTurboReportResultViewModel> Handle(GetBigSmallTurboReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBigSmallTurboReport(
                request.RoundId,
                request.Nickname,
                request.ExcludeBot,
                request.PartnerId,
                request.Username);
            var partners = await partnerService.GetPartnersForSelection(model => true);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, request.Currency);

            return new BigSmallTurboReportResultViewModel
            {
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                    ExchangeRate = exchangeRate,
                },
                Records = result.Records.Select(x => new BigSmallRecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Select = x.Select,
                    Currency = x.Currency,
                    Partner = GetPartnerName(partners, x.PartnerId),
                    Rate = x.Rate,
                })
            };
        }

        public async Task<OddEvenReportResultViewModel> Handle(GetOddEvenReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetOddEvenReport(
                request.RoundId,
                request.Nickname,
                request.ExcludeBot,
                request.PartnerId,
                request.Username);
            var partners = await partnerService.GetPartnersForSelection(model => true);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, request.Currency);

            return new OddEvenReportResultViewModel
            {
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                    ExchangeRate = exchangeRate,
                },
                Records = result.Records.Select(x => new OddEvenRecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Select = x.Select,
                    Currency = x.Currency,
                    Partner = GetPartnerName(partners, x.PartnerId),
                    Rate = x.Rate,
                })
            };
        }

        public async Task<OddEvenTurboReportResultViewModel> Handle(GetOddEvenTurboReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetOddEventTurboReport(
                request.RoundId,
                request.Nickname,
                request.ExcludeBot,
                request.PartnerId,
                request.Username);
            var partners = await partnerService.GetPartnersForSelection(model => true);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, request.Currency);

            return new OddEvenTurboReportResultViewModel
            {
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                    ExchangeRate = exchangeRate,
                },
                Records = result.Records.Select(x => new OddEvenRecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Select = x.Select,
                    Currency = x.Currency,
                    Partner = GetPartnerName(partners, x.PartnerId),
                    Rate = x.Rate,
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


        public async Task<SpecialSeDieReportResultViewModel> Handle(GetSpecialSeDieReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetSpecialSeDieReport(
                request.RoundId,
                request.Nickname,
                request.ExcludeBot,
                request.PartnerId,
                request.Username);
            var partners = await partnerService.GetPartnersForSelection(model => true);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, request.Currency);
            var roundResult = mapper.Map<SeDieRoundResultViewModel>(result.RoundResult);

            roundResult.ExchangeRate = exchangeRate;

            return new SpecialSeDieReportResultViewModel
            {
                RoundResult = roundResult,
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

        public async Task<BlackjackReportResultViewModel> Handle(GetBlackjackReportRequest request, CancellationToken cancellationToken)
        {
            var partners = await partnerService.GetPartnersForSelection(model => true);
            var result = await reportService.GetBlackjackReport(request.Filter);

            return new BlackjackReportResultViewModel
            {
                ConvertToUSD = request.Filter.ConvertToUSD,
                Records = result.Select(x =>
                {
                    var betViewModel = mapper.Map<BlackjackRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    betViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return betViewModel;
                })
            };
        }

        public async Task<SlotGameReportResultViewModel> Handle(GetSlotGameReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetSlotGameReport(request);

            return new SlotGameReportResultViewModel
            {
                ConvertToUSD = request.ConvertToUSD,
                Records = result.Select(x =>
                {
                    var betViewModel = mapper.Map<SlotGameRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    betViewModel.Game = request.Game.Value;
                    betViewModel.ConvertToUSD(request.ConvertToUSD);

                    return betViewModel;
                })
            };
        }

        public async Task<LuckyWheelHistoryViewModel> Handle(GetLuckyWheelHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetLuckyWheelBetHistory(request.Nickname);

            return new LuckyWheelHistoryViewModel(result, appSettings.DefaultTimeZone);
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

        public async Task<BitcoinMasterReportResultViewModel> Handle(GetBitcoinMasterReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBitcoinMasterReport(
                request.RoundId,
                request.Nickname,
                request.ExcludeBot,
                request.PartnerId,
                request.Username);

            var partners = await partnerService.GetPartnersForSelection(model => true);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, request.Currency);

            return new BitcoinMasterReportResultViewModel
            {
                Currency = request.Currency,
                RoundResult = new BitcoinMasterRoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    BettingPrice = result.RoundResult.BettingPrice,
                    ResultPrice = result.RoundResult.ResultPrice,
                    SettlementResult = result.RoundResult.SettlementResult,
                    ExchangeRate = exchangeRate
                },
                Records = result.Records.Select(x => new BitcoinMasterRecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Win = x.Win,
                    Choice = x.Choice,
                    Currency = x.Currency,
                    Partner = GetPartnerName(partners, x.PartnerId),
                    Rate = x.Rate,
                })
            };
        }

        public async Task<BitcoinMasterBetHistoryViewModel> Handle(GetBitcoinMasterBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBitcoinMasterBetHistory(request.RoundId, request.Nickname);

            var partners = await partnerService.GetPartnersForSelection(model => true);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, request.Currency);

            return new BitcoinMasterBetHistoryViewModel
            {
                RoundResult = new BitcoinMasterRoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    BettingPrice = result.RoundResult.BettingPrice,
                    ResultPrice = result.RoundResult.ResultPrice,
                    ExchangeRate = exchangeRate
                },
                Bets = result.Bets.Select(x => new BitcoinMasterBetHistoryRecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                    BetWin = x.BetWin,
                    Refund = x.Refund,
                    Amount = x.Amount,
                    Choice = x.Choice,
                    Win = x.Win,
                    Currency = x.Currency,
                    Partner = GetPartnerName(partners, x.PartnerId),
                    Rate = x.Rate,
                })
            };
        }

        private async Task<decimal> GetExchangeRate(int exchangeRateId, string currency)
        {
            var exchangeRate = await exchangeRateService.GetByExchangeId(exchangeRateId);

            return exchangeRate != null
                   && !string.IsNullOrEmpty(currency)
                   && exchangeRate.Code.ContainsKey(currency) ? exchangeRate.Code[currency] : 1;
        }

        public Task<ReportResultViewModel> Handle(ReportRequest request, CancellationToken cancellationToken)
        {
            var service = reportServiceFactory.GetService(request.Filter.Game);
            return service.Get(request.Filter);
        }

        public async Task<BetHistoryViewModel> Handle(BetHistoryReportRequest request, CancellationToken cancellationToken)
        {
            var service = reportServiceFactory.GetBetHistoryService(request.Filter.Game);
            var viewModel = await service.Get(request.Filter);
            viewModel.ConvertToUSD = request.ConvertToUSD;

            return viewModel;
        }

        public async Task<CasinoHoldemPokerReportResultViewModel> Handle(GetCasinoHoldemPokerReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetCasinoHoldemPokerBetReport(request.Filter);
            var partners = await partnerService.GetPartnersForSelection(model => true);

            var records = result.Select(x =>
            {
                var recordViewModel = mapper.Map<CasinoHoldemPokerRecordViewModel>(x);
                recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                return recordViewModel;
            });

            return new CasinoHoldemPokerReportResultViewModel(request.Filter.ConvertToUSD, records);
        }

        public async Task<CasinoHoldemPokerBetHistoryViewModel> Handle(GetCasinoHoldemPokerBetHistoryRequest request, CancellationToken cancellationToken)
            => new CasinoHoldemPokerBetHistoryViewModel(appSettings.DefaultTimeZone, await reportService.GetCasinoHoldemPokerBetHistory(request.Filter));

        public async Task<ReportSeletionsViewModel> Handle(GetSelectionsRequest request, CancellationToken cancellationToken)
        {
            var parterItems = await partnerService.GetPartnersForSelection(partner => true);
            var currencies = await currencyService.GetCurrenciesForSelection(model => string.IsNullOrEmpty(model.GroupId) && model.Enabled);

            return new ReportSeletionsViewModel
            {
                Partners = parterItems,
                Currencies = currencies
            };
        }

        private static string GetPartnerName(IEnumerable<SelectListItem> partners, string partnerId)
        => partners.FirstOrDefault(option => string.Equals(option.Value, partnerId))?.Text ?? string.Empty;
    }
}
