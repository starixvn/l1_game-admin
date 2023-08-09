using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Currencies;
using GamesAdmin.Site.Features.CurrencyGroupGames;
using GamesAdmin.Site.Features.ExchangeRates;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
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
        IRequestHandler<GetSelectionsRequest, ReportSeletionsViewModel>,
        IRequestHandler<ReportRequest, ReportResultViewModel>,
        IRequestHandler<BetHistoryReportRequest, BetHistoryViewModel>,
        IRequestHandler<GetHiLoReportRequest, HiLoReportResultViewModel>,
        IRequestHandler<HoldemPokerReportRequest, HoldemPokerReportViewModel>,
        IRequestHandler<GetHoldemPokerReportRequest, HoldemPokerReportResultViewModel>,
        IRequestHandler<GetHoldemPokerBetHistoryRequest, HoldemPokerBetHistoryViewModel>,
        IRequestHandler<GetCascadingSlotReportRequest, CascadingSlotReportResultViewModel>,
        IRequestHandler<GetCascadingSlotBetHistoryRequest, CascadingSlotBetHistoryViewModel>
    {
        private readonly IReportService reportService;
        private readonly IAppSettings appSettings;
        private readonly IMapper mapper;
        private readonly IPartnerService partnerService;
        private readonly ICurrencyService currencyService;
        private readonly IExchangeRateService exchangeRateService;
        private readonly IReportServiceFactory reportServiceFactory;
        private readonly ISystemSettings systemSettings;
        private readonly ResourceSiteConfiguration resourceSiteConfig;
        private readonly ICurrencyGroupGameService currencyGroupGameService;

        public ReportHandler(
            IReportService reportService,
            IAppSettings appSettings,
            IMapper mapper,
            IPartnerService partnerService,
            ICurrencyService currencyService,
            IExchangeRateService exchangeRateService,
            IReportServiceFactory reportServiceFactory,
            ISystemSettings systemSettings,
            IOptionsSnapshot<ResourceSiteConfiguration> resourceSiteConfig,
            ICurrencyGroupGameService currencyGroupGameService)
        {
            this.reportService = reportService;
            this.appSettings = appSettings;
            this.mapper = mapper;
            this.partnerService = partnerService;
            this.currencyService = currencyService;
            this.exchangeRateService = exchangeRateService;
            this.reportServiceFactory = reportServiceFactory;
            this.systemSettings = systemSettings;
            this.resourceSiteConfig = resourceSiteConfig.Value;
            this.currencyGroupGameService = currencyGroupGameService;
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
            var currency = await currencyService.GetById(request.Currency);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, currency);

            return new BigSmallReportResultViewModel
            {
                ConvertCurrency = currency.DisplayText,
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                    ExchangeRate = exchangeRate,
                    ExchangeRateId = result.RoundResult.ExchangeRateId
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
            var currency = await currencyService.GetById(request.Currency);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, currency);

            return new BigSmallTurboReportResultViewModel
            {
                ConvertCurrency = currency.DisplayText,
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                    ExchangeRate = exchangeRate,
                    ExchangeRateId = result.RoundResult.ExchangeRateId
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
            var currency = await currencyService.GetById(request.Currency);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, currency);

            return new OddEvenReportResultViewModel
            {
                ConvertCurrency = currency.DisplayText,
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                    ExchangeRate = exchangeRate,
                    ExchangeRateId = result.RoundResult.ExchangeRateId
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
            var currency = await currencyService.GetById(request.Currency);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, currency);

            return new OddEvenTurboReportResultViewModel
            {
                ConvertCurrency = currency.DisplayText,
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                    ExchangeRate = exchangeRate,
                    ExchangeRateId = result.RoundResult.ExchangeRateId
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
                ExchangeRate = request.Rate,
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
                ExchangeRate = request.Rate,
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
                ExchangeRate = request.Rate,
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
                ExchangeRate = request.Rate,
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
            var currency = await currencyService.GetById(request.Currency);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, currency);
            var roundResult = mapper.Map<SeDieRoundResultViewModel>(result.RoundResult);

            roundResult.ExchangeRate = exchangeRate;
            roundResult.ExchangeRateId = result.RoundResult.ExchangeRateId;

            return new SpecialSeDieReportResultViewModel
            {
                ConvertCurrency = currency.DisplayText,
                RoundResult = roundResult,
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<SpecialSeDieRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    recordViewModel.Currency = x.Currency;
                    recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

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
                ExchangeRate = request.Rate,
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

            request.Filter.ToDate.AddDays(1);
            var result = await reportService.GetBlackjackReport(request.Filter);

            return new BlackjackReportResultViewModel
            {
                ConvertToUSD = request.Filter.ConvertToUSD,
                BaseCurrencyMultiplier = systemSettings.BaseCurrencyMultiplier,
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
            var partners = await partnerService.GetPartnersForSelection(model => true);
  
            return new SlotGameReportResultViewModel
            {
                ConvertToUSD = request.ConvertToUSD,
                Records = result.Select(x =>
                {
                    var betViewModel = mapper.Map<SlotGameRecordViewModel>(x);
                    betViewModel.SystemType = systemSettings.System;
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    betViewModel.GameName = request.Game;
                    betViewModel.Partner = GetPartnerName(partners, x.PartnerId);
                    betViewModel.ExchangeRateId = x.ExchangRateId;
                    betViewModel.ConvertToUSD(request.ConvertToUSD, systemSettings.BaseCurrencyMultiplier);
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
            var currencies = await currencyService.GetAll();
            var currencyGroupGame = await currencyGroupGameService.GetAll();
            var stakeLevelBase = GetStakeLevel(currencies, currencyGroupGame, request.GameId, request.BaseCurrency);
            return result.Select(x =>
            {            
                var betViewModel = mapper.Map<JackpotRecordViewModel>(x);
                betViewModel.BaseStakeLevel = stakeLevelBase;
                betViewModel.StakeLevelByCurrency = GetStakeLevel(currencies, currencyGroupGame, request.GameId, x.Currency);             
                betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                betViewModel.ConvertToUserCurrency(!request.ConvertToINH, systemSettings.BaseCurrencyMultiplier);
                return betViewModel;
            }).OrderByDescending(x => x.Time);
        }

        private static StakeLevel[] GetStakeLevel(IEnumerable<CurrencyModel> currencies, IEnumerable<CurrencyGroupGame> currencyGroupGame, int gameId, string name)
        {
            var currency = currencies.FirstOrDefault(x => x.Name == name);
            var currencyGame = currencyGroupGame.FirstOrDefault(currencyGroup => currencyGroup.GameId == gameId && currencyGroup.GroupId == currency.GroupId);
            return currencyGame?.StakeLevels;
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
            var currency = await currencyService.GetById(request.Currency);
            var exchangeRate = await GetExchangeRate(result.RoundResult.ExchangeRateId, currency);

            return new BitcoinMasterReportResultViewModel
            {
                Currency = currency.DisplayText,
                RoundResult = new BitcoinMasterRoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    BettingPrice = result.RoundResult.BettingPrice,
                    ResultPrice = result.RoundResult.ResultPrice,
                    SettlementResult = result.RoundResult.SettlementResult,
                    ExchangeRate = exchangeRate,
                    ExchangeRateId = result.RoundResult.ExchangeRateId
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

            return new BitcoinMasterBetHistoryViewModel
            {
                ExchangeRate = request.Rate,
                RoundResult = new BitcoinMasterRoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    BettingPrice = result.RoundResult.BettingPrice,
                    ResultPrice = result.RoundResult.ResultPrice,
                    ExchangeRate = request.Rate ?? 0,
                    ExchangeRateId = result.RoundResult.ExchangeRateId
                },
                Bets = result.Bets.Select(x => new BitcoinMasterBetHistoryRecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                    BetWin = x.BaseBetWin,
                    Refund = x.BaseRefund,
                    Amount = x.BaseAmount,
                    Choice = x.Choice,
                    Win = x.Win,
                    Currency = x.Currency,
                    Partner = GetPartnerName(partners, x.PartnerId),
                    Rate = x.Rate,
                })
            };
        }

        private async Task<decimal> GetExchangeRate(int exchangeRateId, CurrencyModel currency)
        {
            var code = currency.Code;
            var exchangeRate = await exchangeRateService.GetByExchangeId(exchangeRateId);

            var rate = string.IsNullOrEmpty(code) || exchangeRate == null || !exchangeRate.Code.ContainsKey(code)
                ? 1 : exchangeRate.Code[code];

            return rate * ((decimal)currency.Multiplier);
        }

        public async Task<ReportResultViewModel> Handle(ReportRequest request, CancellationToken cancellationToken)
        {
            var service = reportServiceFactory.GetService(request.Filter.Game);
            var result = await service.Get(request.Filter);

            result.BaseCurrency = systemSettings.BaseCurrency;
            result.BaseCurrencyMultiplier = systemSettings.BaseCurrencyMultiplier;
            result.GameName = Game.FromGameId(request.Filter.Game).Value;

            return result;
        }

        public async Task<BetHistoryViewModel> Handle(BetHistoryReportRequest request, CancellationToken cancellationToken)
        {
            var service = reportServiceFactory.GetBetHistoryService(request.Filter.Game);
            var viewModel = await service.Get(request.Filter);
            viewModel.ConvertToUSD = request.ConvertToUSD;
            viewModel.BaseCurrencyMultiplier = systemSettings.BaseCurrencyMultiplier;

            return viewModel;
        }

        public async Task<ReportSeletionsViewModel> Handle(GetSelectionsRequest request, CancellationToken cancellationToken)
        {
            var parterItems = await partnerService.GetPartnersForSelection(partner => true);
            var currencies = (await currencyService.GetCurrenciesForSelection(
                model => model.Enabled,
                model => new SelectListItem($"{model.Name} ({model.Code})", model.Id)))
                .GroupBy(model => model.Text).Select(x => x.First());

            return new ReportSeletionsViewModel
            {
                Partners = parterItems,
                Currencies = currencies
            };
        }

        public async Task<HiLoReportResultViewModel> Handle(GetHiLoReportRequest request, CancellationToken cancellationToken)
        {
            var partners = await partnerService.GetPartnersForSelection(model => true);

            request.Filter.ToDate = request.Filter.ToDate.AddDays(1);
            var result = await reportService.GetHiLoReport(request.Filter);

            return new HiLoReportResultViewModel
            {
                ConvertToUSD = request.Filter.ConvertToUSD,
                BaseCurrencyMultiplier = systemSettings.BaseCurrencyMultiplier,
                Records = result.Select(x =>
                {
                    var betViewModel = mapper.Map<HiLoRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    betViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return betViewModel;
                })
            };
        }

        public async Task<HoldemPokerReportViewModel> Handle(HoldemPokerReportRequest request, CancellationToken cancellationToken)
        {
            var selections = new ReportSeletionsViewModel
            {
                Partners = await partnerService.GetPartnersForSelection(partner => true),
                Currencies = await currencyService.GetCurrenciesForSelection(model => string.IsNullOrEmpty(model.GroupId) && model.Enabled)
            };

            return new HoldemPokerReportViewModel { BaseCurrency = this.systemSettings.BaseCurrency, ExcludeUUS = true, Selections = selections, Game = Game.FromGameId(request.GameId) };
        }

        public async Task<HoldemPokerReportResultViewModel> Handle(GetHoldemPokerReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetHoldemPokerBetReport(request.Filter);
            var partners = await partnerService.GetPartnersForSelection(model => true);

            var records = result.Select(x =>
            {
                var recordViewModel = mapper.Map<HoldemPokerRecordViewModel>(x);
                recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                return recordViewModel;
            });

            return new HoldemPokerReportResultViewModel(request.Filter.ConvertToUSD, request.Filter.GameId, records, systemSettings.BaseCurrencyMultiplier);
        }

        public async Task<HoldemPokerBetHistoryViewModel> Handle(GetHoldemPokerBetHistoryRequest request, CancellationToken cancellationToken)
            => new HoldemPokerBetHistoryViewModel(appSettings.DefaultTimeZone, await reportService.GetHoldemPokerBetHistory(request.Filter), systemSettings.BaseCurrencyMultiplier);

        private static string GetPartnerName(IEnumerable<SelectListItem> partners, string partnerId)
        => partners.FirstOrDefault(option => string.Equals(option.Value, partnerId))?.Text ?? string.Empty;

        public async Task<CascadingSlotReportResultViewModel> Handle(GetCascadingSlotReportRequest request, CancellationToken cancellationToken)
        {
            var filter = new CascadingSlotReportFilter
            {
                RoundId = request.RoundId,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Nickname = request.Nickname,
                ExcludeUUS = request.ExcludeUUS,
                PartnerId = request.PartnerId,
                Username = request.Username,
                ConvertToUSD = request.ConvertToUSD,
                Game = request.Game,
            };

            var result = await reportService.GetCascadingSlotReport(filter);
            var partners = await partnerService.GetPartnersForSelection(model => true);

            return new CascadingSlotReportResultViewModel
            {
                ConvertToUSD = request.ConvertToUSD,
                Records = result.Select(x =>
                {
                    var betViewModel = mapper.Map<CascadingSlotRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    betViewModel.Game = request.Game;
                    betViewModel.Partner = GetPartnerName(partners, x.PartnerId);
                    betViewModel.ConvertToUSD(request.ConvertToUSD, systemSettings.BaseCurrencyMultiplier);

                    return betViewModel;
                })
            };
        }

        public async Task<CascadingSlotBetHistoryViewModel> Handle(GetCascadingSlotBetHistoryRequest request, CancellationToken cancellationToken)
        {
            var filter = new CascadingSlotReportFilter
            {
                RoundId = request.RoundId,
                Nickname = request.Nickname,
                ExcludeUUS = request.ExcludeUUS,
                ConvertToUSD = request.ConvertToUSD,
                Game = request.Game,
            };

            var result = await reportService.GetCascadingSlotBetHistory(filter);
            var viewModel = new CascadingSlotBetHistoryViewModel
            {
                Game = request.Game,
                StaticResourceUrl = resourceSiteConfig.BaseUrl,
                ConvertToUSD = request.ConvertToUSD,
                Round = mapper.Map<CascadingSlotRecordViewModel>(result.Round),
                Multipliers = result.Multipliers.Select(x => mapper.Map<CascadingSlotRoundMultiplierViewModel>(x))
            };

            viewModel.Round.ConvertToUSD(request.ConvertToUSD, systemSettings.BaseCurrencyMultiplier);

            return viewModel;
        }
    }
}