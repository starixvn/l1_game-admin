using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.GeneralGameSettings;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Core.Models.Spins;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Spins;
using GamesAdmin.Database.Report;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.Report
{
    public interface IReportService
    {
        Task<BigSmallBetReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<IEnumerable<BaseBetHistory>> GetBetHistoryReport(string name, long roundId, int gameType);

        Task<BigSmallBetReport> GetBigSmallTurboReportAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<OddEvenBetReport> GetOddEvenReportAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<OddEvenBetReport> GetOddEvenTurboReportAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<IEnumerable<SpinResult>> GetLuckyWheelReportAsync(string nickname);

        Task<SpecialSeDieBetReport> GetSpecialSeDieReportAsync(long roundId, string nickname, bool excludeUUS, string partnerId, string username);

        Task<SpecialSeDieBetHistory> GetSpecialSeDieBetHistoryAsync(long roundId, string userId);

        Task<IEnumerable<BlackjackBetRecord>> GetBlackjackReportAsync(BlackjackBetReportFilter filter);

        Task<IEnumerable<BlackjackBetRecord>> GetPontoonReportAsync(BlackjackBetReportFilter filter);

        Task<IEnumerable<SlotGameBetRecord>> GetSlotGameReportAsync(SlotGameBetReportFilter filter);

        Task<IEnumerable<JackpotRecord>> GetJackpotReportAsync(int gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot);

        Task<BitcoinMasterBetReport> GetBitcoinMasterReportAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<BitcoinMasterBetHistory> GetBitcoinMasterBetHistoryAsync(long roundId, string nickname);

        Task<IEnumerable<CascadingSlotBetRecord>> GetCascadingSlotReportAsync(CascadingSlotReportFilter filter);

        Task<CascadingSlotBetHistory> GetCascadingSlotBetHistoryAsync(CascadingSlotReportFilter filter);

        Task<CrashGameBetReport> GetCrashGameReportAsync(SpeedUpGameBetReportFilter filter);
    }

    public class ReportService : IReportService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IGenericRepository<SpinResultEntity> spinResultRepository;
        private readonly IMapper mapper;
        private readonly ICascadingSlotGameReportRepository cascadingSlotGameReportRepository;
        private readonly ISlotGameReportRepository slotGameReportRepository;
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly ICrashGameReportRepository crashGameReportRepository;

        public ReportService(
            IServiceProvider serviceProvider,
            IMapper mapper,
            IGenericRepository<SpinResultEntity> spinResultRepository,
            ICascadingSlotGameReportRepository cascadingSlotGameReportRepository,
            ISlotGameReportRepository slotGameReportRepository,
            IGeneralGameSettingService generalGameSettingService,
            ICrashGameReportRepository crashGameReportRepository)
        {
            this.serviceProvider = serviceProvider;
            this.mapper = mapper;
            this.spinResultRepository = spinResultRepository;
            this.cascadingSlotGameReportRepository = cascadingSlotGameReportRepository;
            this.slotGameReportRepository = slotGameReportRepository;
            this.generalGameSettingService = generalGameSettingService;
            this.crashGameReportRepository = crashGameReportRepository;
        }

        public async Task<BigSmallBetReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        {
            var repository = serviceProvider.GetService<IStakeBalancedReportRepository>();
            var result = await repository.GetAsync<BigSmallRoundEntity, BigSmallBetEntity>(roundId, nickname, excludeBot, partnerId, username, "big");

            return new BigSmallBetReport
            {
                RoundResult = new BigSmallRoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Result?.Dice1 ?? 0,
                    Dice2 = result.Item1.Result?.Dice2 ?? 0,
                    Dice3 = result.Item1.Result?.Dice3 ?? 0,
                    ExchangeRateId = result.Item1.ExchangeRateId,
                },
                Records = result.Item2.Select(x => new BigSmallBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Select = x.Select,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back,
                    Currency = x.Currency,
                    PartnerId = x.PartnerId,
                    Rate = x.Rate
                })
            };
        }

        public async Task<BigSmallBetReport> GetBigSmallTurboReportAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        {
            var repository = serviceProvider.GetService<IStakeBalancedReportRepository>();
            var result = await repository.GetAsync<BigSmallTurboRoundEntity, BigSmallTurboBetEntity>(roundId, nickname, excludeBot, partnerId, username, "big");

            return new BigSmallBetReport
            {
                RoundResult = new BigSmallRoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1 ?? result.Item1.Result?.Dice1 ?? 0,
                    Dice2 = result.Item1.Dice2 ?? result.Item1.Result?.Dice2 ?? 0,
                    Dice3 = result.Item1.Dice3 ?? result.Item1.Result?.Dice3 ?? 0,
                    ExchangeRateId = result.Item1.ExchangeRateId,
                },
                Records = result.Item2.Select(x => new BigSmallBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Select = x.Select,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back,
                    Currency = x.Currency,
                    PartnerId = x.PartnerId,
                    Rate = x.Rate
                })
            };
        }

        public async Task<OddEvenBetReport> GetOddEvenReportAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        {
            var repository = serviceProvider.GetService<IStakeBalancedReportRepository>();
            var result = await repository.GetAsync<OddEvenRoundEntity, OddEvenBetEntity>(roundId, nickname, excludeBot, partnerId, username, "even");

            return new OddEvenBetReport
            {
                RoundResult = new OddEvenRoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1,
                    Dice2 = result.Item1.Dice2,
                    Dice3 = result.Item1.Dice3,
                    ExchangeRateId = result.Item1.ExchangeRateId,
                },
                Records = result.Item2.Select(x => new OddEvenBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Select = x.Select,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back,
                    Currency = x.Currency,
                    PartnerId = x.PartnerId,
                    Rate = x.Rate
                })
            };
        }

        public async Task<OddEvenBetReport> GetOddEvenTurboReportAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        {
            var repository = serviceProvider.GetService<IStakeBalancedReportRepository>();
            var result = await repository.GetAsync<OddEvenTurboRoundEntity, OddEvenTurboBetEntity>(roundId, nickname, excludeBot, partnerId, username, "even");

            return new OddEvenBetReport
            {
                RoundResult = new OddEvenRoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1 ?? result.Item1.Result?.Dice1 ?? 0,
                    Dice2 = result.Item1.Dice2 ?? result.Item1.Result?.Dice2 ?? 0,
                    Dice3 = result.Item1.Dice3 ?? result.Item1.Result?.Dice3 ?? 0,
                    ExchangeRateId = result.Item1.ExchangeRateId,
                },
                Records = result.Item2.Select(x => new OddEvenBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Select = x.Select,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back,
                    Currency = x.Currency,
                    PartnerId = x.PartnerId,
                    Rate = x.Rate
                })
            };
        }

        public async Task<IEnumerable<BaseBetHistory>> GetBetHistoryReport(string name, long roundId, int gameType)
        {
            switch (gameType)
            {
                case (int)GameId.BigSmall:
                    return await GetBigSmallBetHistoryAsync(name, roundId);

                case (int)GameId.BigSmallTurbo:
                    return await GetBigSmallTurboBetHistoryAsync(name, roundId);

                case (int)GameId.OddEven:
                    return await GetOddEvenBetHistoryAsync(name, roundId);

                case (int)GameId.OddEvenTurbo:
                    return await GetOddEvenTurboBetHistoryAsync(name, roundId);

                default:
                    break;
            }

            return await GetBigSmallBetHistoryAsync(name, roundId);
        }

        private async Task<IEnumerable<BigSmallBet>> GetBigSmallBetHistoryAsync(string name, long roundId)
        {
            var betHistoryReportRepository = serviceProvider.GetService<IBetHistoryReportRepository>();
            var result = await betHistoryReportRepository.GetBigSmallAsync(roundId, name);

            return result.Select(x =>
            {
                var betResult = new BigSmallBetResult(x.FirstDice, x.SecondDice, x.ThirdDice);
                var betChoice = new BigSmallChoice(x.Select, x.Bet);

                return new BigSmallBet(x.Round, x.Time, x.BetWin, betResult, betChoice, x.Refund);
            });
        }

        private async Task<IEnumerable<BigSmallBet>> GetBigSmallTurboBetHistoryAsync(string name, long roundId)
        {
            var betHistoryReportRepository = serviceProvider.GetService<IBetHistoryReportRepository>();
            var result = await betHistoryReportRepository.GetBigSmallTurboAsync(roundId, name);

            return result.Select(x =>
            {
                var betResult = new BigSmallBetResult(x.FirstDice, x.SecondDice, x.ThirdDice);
                var betChoice = new BigSmallChoice(x.Select, x.Bet);

                return new BigSmallBet(x.Round, x.Time, x.BetWin, betResult, betChoice, x.Refund);
            });
        }

        private async Task<IEnumerable<OddEvenBet>> GetOddEvenBetHistoryAsync(string name, long roundId)
        {
            var betHistoryReportRepository = serviceProvider.GetService<IBetHistoryReportRepository>();
            var result = await betHistoryReportRepository.GetOddEvenAsync(roundId, name);

            return result.Select(x =>
            {
                var betResult = new OddEvenBetResult(x.FirstDice, x.SecondDice, x.ThirdDice);
                var betChoice = new OddEvenChoice(x.Select, x.Bet);

                return new OddEvenBet(x.Round, x.Time, x.BetWin, betResult, betChoice, x.Refund);
            });
        }

        private async Task<IEnumerable<OddEvenBet>> GetOddEvenTurboBetHistoryAsync(string name, long roundId)
        {
            var betHistoryReportRepository = serviceProvider.GetService<IBetHistoryReportRepository>();
            var result = await betHistoryReportRepository.GetOddEvenTurboAsync(roundId, name);

            return result.Select(x =>
            {
                var betResult = new OddEvenBetResult(x.FirstDice, x.SecondDice, x.ThirdDice);
                var betChoice = new OddEvenChoice(x.Select, x.Bet);

                return new OddEvenBet(x.Round, x.Time, x.BetWin, betResult, betChoice, x.Refund);
            });
        }

        public async Task<IEnumerable<SpinResult>> GetLuckyWheelReportAsync(string nickname)
        {
            var results = await spinResultRepository.FilterByAsync(spin => spin.Name == nickname);

            return mapper.Map<IEnumerable<SpinResult>>(results.ToList());
        }

        public async Task<SpecialSeDieBetReport> GetSpecialSeDieReportAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        {
            var repository = serviceProvider.GetService<IStakeBalancedReportRepository>();
            var result = await repository.GetAsync<SpecialSeDieRoundEntity, SpecialSeDieBetEntity>(roundId, nickname, excludeBot, partnerId, username, "");

            return new SpecialSeDieBetReport
            {
                RoundResult = mapper.Map<SeDieRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<SpecialSeDieBetRecord>(x))
            };
        }

        public async Task<SpecialSeDieBetHistory> GetSpecialSeDieBetHistoryAsync(long roundId, string userId)
        {
            var repository = serviceProvider.GetService<IBetHistoryReportRepository>();
            var result = await repository.GetSpecialSeDieAsync(roundId, userId);

            return new SpecialSeDieBetHistory
            {
                RoundResult = mapper.Map<SeDieRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<SpecialSeDieBetHistoryRecord>(x))
            };
        }

        public async Task<IEnumerable<BlackjackBetRecord>> GetBlackjackReportAsync(BlackjackBetReportFilter filter)
        {
            var repository = serviceProvider.GetService<IBlackjackReportRepository>();
            var result = await repository.GetAsync(filter);

            return result.Select(x => mapper.Map<BlackjackBetRecord>(x));
        }

        public async Task<IEnumerable<BlackjackBetRecord>> GetPontoonReportAsync(BlackjackBetReportFilter filter)
        {
            var repository = serviceProvider.GetService<IPontoonReportRepository>();
            var result = await repository.GetAsync(filter);

            return result.Select(x => mapper.Map<BlackjackBetRecord>(x));
        }

        public async Task<IEnumerable<SlotGameBetRecord>> GetSlotGameReportAsync(SlotGameBetReportFilter filter)
        {
            var games = await generalGameSettingService.GetAll();

            filter.GameInfo = games.FirstOrDefault(game => game.Name == filter.Game);

            var result = await slotGameReportRepository.GetAsync(filter);

            return result.Select(x => mapper.Map<SlotGameBetRecord>(x));
        }

        public async Task<IEnumerable<JackpotRecord>> GetJackpotReportAsync(int gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IJackpotRepository>();
            var result = await repository.GetAsync(gameId, fromDate, toDate, excludeUUS, excludeBot);

            return result.Select(x => mapper.Map<JackpotRecord>(x));
        }

        public async Task<BitcoinMasterBetReport> GetBitcoinMasterReportAsync(
            long roundId, string nickname, bool excludeBot, string partnerId, string username)
        {
            var repository = serviceProvider.GetService<IStakeBalancedReportRepository>();
            var result = await repository.GetAsync<BitcoinMasterRoundEntity, BitcoinMasterBetEntity>(roundId, nickname, excludeBot, partnerId, username, "");

            return new BitcoinMasterBetReport
            {
                RoundResult = new BitcoinMasterRoundResult
                {
                    Number = result.Item1.Number,
                    ResultPrice = result.Item1.Result?.ResultPrice ?? 0,
                    BettingPrice = result.Item1.Result?.BettingPrice ?? 0,
                    SettlementResult = result.Item1.SettlementResult,
                    ExchangeRateId = result.Item1.ExchangeRateId,
                },
                Records = result.Item2.Select(x => new BitcoinMasterBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back,
                    Win = x.Win,
                    Choice = x.Choice,
                    Currency = x.Currency,
                    PartnerId = x.PartnerId,
                    Rate = x.Rate
                })
            };
        }

        public async Task<BitcoinMasterBetHistory> GetBitcoinMasterBetHistoryAsync(long roundId, string nickname)
        {
            var repository = serviceProvider.GetService<IBetHistoryReportRepository>();
            var result = await repository.GetBitcoinMasterBetHistoryAsync(roundId, nickname);
            var model = new BitcoinMasterBetHistory
            {
                RoundResult = mapper.Map<BitcoinMasterRoundResult>(result.Item1.Result),
                Bets = result.Item2.Select(x => mapper.Map<BitcoinMasterBetHistoryRecord>(x))
            };

            model.RoundResult.Number = result.Item1.Number;
            model.RoundResult.ExchangeRateId = result.Item1.ExchangeRateId;
            return model;
        }

        public async Task<IEnumerable<CascadingSlotBetRecord>> GetCascadingSlotReportAsync(CascadingSlotReportFilter filter)
        {
            var games = await generalGameSettingService.GetAll();

            filter.GameInfo = games.FirstOrDefault(game => game.Name == filter.Game);

            var result = await cascadingSlotGameReportRepository.GetRecordAsync(filter);
            return result.Select(x => mapper.Map<CascadingSlotBetRecord>(x));
        }

        public async Task<CascadingSlotBetHistory> GetCascadingSlotBetHistoryAsync(CascadingSlotReportFilter filter)
        {
            var games = await generalGameSettingService.GetAll();

            filter.GameInfo = games.FirstOrDefault(game => game.Name == filter.Game);
            var result = await cascadingSlotGameReportRepository.GetBetHistoryAsync(filter);
            return new CascadingSlotBetHistory
            {
                Round = mapper.Map<CascadingSlotBetRecord>(result.Round),
                Multipliers = result.Multipliers.Select(x => mapper.Map<CascadingSlotRoundMultiplier>(x))
            };
        }

        public async Task<CrashGameBetReport> GetCrashGameReportAsync(SpeedUpGameBetReportFilter filter)
        {
            var games = await generalGameSettingService.GetAll();

            filter.GameInfo = games.FirstOrDefault(game => game.Name == filter.Game);

            var result = await crashGameReportRepository.GetRecordAsync(filter);

            return new CrashGameBetReport
            {
                RoundResult = mapper.Map<CrashGameRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<CrashGameBetRecord>(x))
            };
        }
    }
}