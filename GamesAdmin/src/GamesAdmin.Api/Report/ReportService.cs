using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Report.Extensions;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Core.Models.Spins;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Spins;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.Report
{
    public interface IReportService
    {
        Task<BigSmallBetReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot);

        Task<IEnumerable<BaseBetHistory>> GetBetHistoryReport(string name, long roundId, byte gameType);

        Task<BigSmallBetReport> GetBigSmallTurboReportAsync(long roundId, string nickname, bool excludeBot);

        Task<OddEvenBetReport> GetOddEvenReportAsync(long roundId, string nickname, bool excludeBot);

        Task<OddEvenBetReport> GetOddEvenTurboReportAsync(long roundId, string nickname, bool excludeBot);

        Task<SeDieBetReport> GetSeDieReportAsync(long roundId, string nickname, bool excludeUUS);

        Task<SeDieBetReport> GetMiniSeDieReportAsync(long roundId, string nickname, bool excludeUUS);

        Task<IEnumerable<KenoBetRecord>> GetKenoReportAsync(GameType game, long roundId, string nickname, bool excludeUUS = false);

        Task<IEnumerable<KenoBetHistory>> GetKenoBetHistoryAsync(GameType game, long roundId, string nickname);

        Task<KenoRoundResult> GetKenoRoundResultAsync(GameType game, long roundId);

        Task<FishPrawnCrabBetReport> GetFishPrawnCrabReportAsync(long roundId, string nickname, bool excludeUUS);

        Task<DragonTigerBetReport> GetDragonTigerReportAsync(long roundId, string nickname, bool excludeUUS);

        Task<BaccaratBetReport> GetBaccaratReportAsync(long roundId, string nickname, bool excludeUUS);

        Task<RouletteBetReport> GetRouletteReportAsync(long roundId, string nickname, bool excludeUUS);

        Task<RouletteBetHistory> GetRouletteBetHistoryAsync(long roundId, string userId);

        Task<IEnumerable<SpinResult>> GetLuckyWheelReportAsync(string nickname);

        Task<VipSeDieBetReport> GetVipSeDieReportAsync(long roundId, string nickname, bool excludeUUS);

        Task<SpecialSeDieBetReport> GetSpecialSeDieReportAsync(long roundId, string nickname, bool excludeUUS);

        Task<SpecialSeDieBetHistory> GetSpecialSeDieBetHistoryAsync(long roundId, string userId);
        
        Task<IEnumerable<BlackjackBetRecord>> GetBlackjackReportAsync(long handId, string nickname, bool excludeUUS);

        Task<VipFishPrawnCrabBetReport> GetVipFishPrawnCrabReportAsync(long roundId, string nickname, bool excludeUUS);

    }

    public class ReportService : IReportService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IKenoReportFactory kenoReportFactory;
        private readonly IGenericRepository<SpinResultEntity> spinResultRepository;
        private readonly IMapper mapper;

        public ReportService(
            IServiceProvider serviceProvider,
            IKenoReportFactory kenoReportFactory,
            IMapper mapper,
            IGenericRepository<SpinResultEntity> spinResultRepository)
        {
            this.serviceProvider = serviceProvider;
            this.kenoReportFactory = kenoReportFactory;
            this.mapper = mapper;
            this.spinResultRepository = spinResultRepository;
        }

        public async Task<BigSmallBetReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IBigSmallReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return new BigSmallBetReport
            {
                RoundResult = new BigSmallRoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1,
                    Dice2 = result.Item1.Dice2,
                    Dice3 = result.Item1.Dice3
                },
                Records = result.Item2.Select(x => new BigSmallBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Select = x.Select,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back
                })
            };
        }

        public async Task<BigSmallBetReport> GetBigSmallTurboReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IBigSmallTurboReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return new BigSmallBetReport
            {
                RoundResult = new BigSmallRoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1,
                    Dice2 = result.Item1.Dice2,
                    Dice3 = result.Item1.Dice3
                },
                Records = result.Item2.Select(x => new BigSmallBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Select = x.Select,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back
                })
            };
        }

        public async Task<OddEvenBetReport> GetOddEvenReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IOddEvenReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return new OddEvenBetReport
            {
                RoundResult = new OddEvenRoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1,
                    Dice2 = result.Item1.Dice2,
                    Dice3 = result.Item1.Dice3
                },
                Records = result.Item2.Select(x => new OddEvenBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Select = x.Select,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back
                })
            };
        }

        public async Task<OddEvenBetReport> GetOddEvenTurboReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<IOddEvenTurboReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

            return new OddEvenBetReport
            {
                RoundResult = new OddEvenRoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1,
                    Dice2 = result.Item1.Dice2,
                    Dice3 = result.Item1.Dice3
                },
                Records = result.Item2.Select(x => new OddEvenBetRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    Select = x.Select,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back
                })
            };
        }

        public async Task<SeDieBetReport> GetSeDieReportAsync(long roundId, string nickname, bool excludeUUS)
        {
            var repository = serviceProvider.GetService<ISeDieReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeUUS);

            return new SeDieBetReport
            {
                RoundResult = result.Item1.ToModel(),
                Records = result.Item2.Select(x => x.ToModel())
            };
        }

        public async Task<SeDieBetReport> GetMiniSeDieReportAsync(long roundId, string nickname, bool excludeUUS)
        {
            var repository = serviceProvider.GetService<IMiniSeDieReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeUUS);

            return new SeDieBetReport
            {
                RoundResult = result.Item1.ToModel(),
                Records = result.Item2.Select(x => x.ToModel())
            };
        }

        public async Task<IEnumerable<BaseBetHistory>> GetBetHistoryReport(string name, long roundId, byte gameType)
        {
            switch (gameType)
            {
                case (byte)GameId.BigSmall:
                    return await GetBigSmallBetHistoryAsync(name, roundId);

                case (byte)GameId.BigSmallTurbo:
                    return await GetBigSmallTurboBetHistoryAsync(name, roundId);

                case (byte)GameId.OddEven:
                    return await GetOddEvenBetHistoryAsync(name, roundId);

                case (byte)GameId.OddEvenTurbo:
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

        public Task<IEnumerable<KenoBetRecord>> GetKenoReportAsync(GameType game, long roundId, string nickname, bool excludeUUS = false)
        {
            var service = kenoReportFactory.GetReportService(game);

            return service == default ? default : service.GetReportAsync(roundId, nickname, excludeUUS);
        }

        public Task<IEnumerable<KenoBetHistory>> GetKenoBetHistoryAsync(GameType game, long roundId, string nickname)
        {
            var service = kenoReportFactory.GetReportService(game);

            return service == default ? default : service.GetBetHistoryAsync(roundId, nickname);
        }

        public Task<KenoRoundResult> GetKenoRoundResultAsync(GameType game, long roundId)
        {
            var service = kenoReportFactory.GetReportService(game);

            return service == default ? default : service.GetRoundResultAsync(roundId);
        }

        public async Task<FishPrawnCrabBetReport> GetFishPrawnCrabReportAsync(long roundId, string nickname, bool excludeUUS)
        {
            var repository = serviceProvider.GetService<IFishPrawnCrabReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeUUS);

            return new FishPrawnCrabBetReport
            {
                RoundResult = mapper.Map<FishPrawnCrabRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<FishPrawnCrabBetRecord>(x))
            };
        }

        public async Task<DragonTigerBetReport> GetDragonTigerReportAsync(long roundId, string nickname, bool excludeUUS)
        {
            var repository = serviceProvider.GetService<IDragonTigerReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeUUS);

            return new DragonTigerBetReport
            {
                RoundResult = mapper.Map<DragonTigerRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<DragonTigerBetRecord>(x))
            };
        }

        public async Task<BaccaratBetReport> GetBaccaratReportAsync(long roundId, string nickname, bool excludeUUS)
        {
            var repository = serviceProvider.GetService<IBaccaratReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeUUS);

            return new BaccaratBetReport
            {
                RoundResult = mapper.Map<BaccaratRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<BaccaratBetRecord>(x))
            };
        }
        public async Task<RouletteBetReport> GetRouletteReportAsync(long roundId, string nickname, bool excludeUUS)
        {
            var repository = serviceProvider.GetService<IRouletteReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeUUS);

            return new RouletteBetReport
            {
                RoundResult = mapper.Map<RouletteRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<RouletteBetRecord>(x))
            };
        }

        public async Task<RouletteBetHistory> GetRouletteBetHistoryAsync(long roundId, string userId)
        {
            var repository = serviceProvider.GetService<IBetHistoryReportRepository>();
            var result = await repository.GetRouletteAsync(roundId, userId);

            return new RouletteBetHistory
            {
                RoundResult = mapper.Map<RouletteRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<RouletteBetHistoryRecord>(x))
            };
        }

        public async Task<IEnumerable<SpinResult>> GetLuckyWheelReportAsync(string nickname)
        {
            var results = await spinResultRepository.FilterByAsync(spin => spin.Name == nickname);

            return mapper.Map<IEnumerable<SpinResult>>(results.ToList());
        }

        public async Task<VipSeDieBetReport> GetVipSeDieReportAsync(long roundId, string nickname, bool excludeUUS)
        {
            var repository = serviceProvider.GetService<IVipSeDieReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeUUS);

            return new VipSeDieBetReport
            {
                RoundResult = mapper.Map<SeDieRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<VipSeDieBetRecord>(x))
            };
        }

        public async Task<SpecialSeDieBetReport> GetSpecialSeDieReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var repository = serviceProvider.GetService<ISpecialSeDieReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeBot);

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

        public async Task<VipFishPrawnCrabBetReport> GetVipFishPrawnCrabReportAsync(long roundId, string nickname, bool excludeUUS)
        {
            var repository = serviceProvider.GetService<IVipFishPrawnCrabReportRepository>();
            var result = await repository.GetAsync(roundId, nickname, excludeUUS);

            return new VipFishPrawnCrabBetReport
            {
                RoundResult = mapper.Map<VipFishPrawnCrabRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<VipFishPrawnCrabBetRecord>(x))
            };
        }

        public async Task<IEnumerable<BlackjackBetRecord>> GetBlackjackReportAsync(long handId, string nickname, bool excludeUUS)
        {
            var repository = serviceProvider.GetService<IBlackjackReportRepository>();
            var result = await repository.GetAsync(handId, nickname, excludeUUS);

            return result.Select(x => mapper.Map<BlackjackBetRecord>(x));
        }
    }
}