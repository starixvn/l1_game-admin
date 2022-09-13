using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Keno;

namespace GamesAdmin.Api.Report
{
    public class MiniKenoReportService<TMiniKenoRound, TMiniKenoBet, TMiniKenoBetLog> : IGameReportService, IBetHistoryReportService
        where TMiniKenoRound : BaseKenoRoundEntity, new()
        where TMiniKenoBet : BaseKenoOnesEntity
        where TMiniKenoBetLog : BaseKenoBetEntity
    {
        private readonly IMiniKenoReportRepository repository;
        private readonly IGenericRepository<TMiniKenoBetLog> betLogRepository;

        public MiniKenoReportService(IMiniKenoReportRepository repository, IGenericRepository<TMiniKenoBetLog> betLogRepository)
        {
            this.repository = repository;
            this.betLogRepository = betLogRepository;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync<TMiniKenoRound, TMiniKenoBet>(filter);

            return new MiniKenoBetReport
            {
                RoundResult = result.Item1.Number == 0 ? default : new KenoRoundResult(
                    result.Item1.Result?.ToArray(),
                    new KenoSettlementResult(result.Item1.SettlementResult),
                     result.Item1.Time)
                {
                    Number = result.Item1.Number,
                    ExchangeRateId = result.Item1.ExchangeRateId
                },
                Records = result.Item2.Select(bet => new KenoBetRecord(bet.ConvertAmountToDictionary(), bet.Odds)
                {
                    UserId = bet.UserId,
                    Nickname = bet.Nickname,
                    Username = bet.Username,
                    BetWin = bet.BetWin,
                    Time = bet.Time,
                    Currency = bet.Currency,
                    PartnerId = bet.PartnerId,
                    Rate = bet.Rate
                })
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var bets = await betLogRepository.FilterByAsync(
                bet => bet.Round == filter.RoundId && bet.Uid == filter.UserId && bet.OperatorId == filter.PartnerId);

            var betLogs = bets
                .Select(bet => new KenoBetHistoryRecord
                {
                    Round = bet.Round,
                    Name = bet.Name,
                    Choice = Enumeration.FromValue<KenoBetChoice>(bet.Choice),
                    Amount = bet.Amount,
                    Time = bet.Time,
                    Currency = bet.Currency,
                    PartnerId = bet.OperatorId,
                    Rate = bet.Rate
                }).OrderByDescending(bet => bet.Time);

            return new KenoBetHistory
            {
                Bets = betLogs
            };
        }
    }
}