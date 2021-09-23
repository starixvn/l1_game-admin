using GamesAdmin.Api.GameUser;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Keno;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public class KenoWestReport : IKenoReport
    {
        private readonly IGenericRepository<KenoWestOnesEntity> kenoWestOnesRepository;
        private readonly IGenericRepository<KenoWestRoundEntity> kenoWestRoundRepository;
        private readonly IGenericRepository<KenoWestBetEntity> kenoWestBetRepository;

        private readonly IGameUserService accountService;

        public KenoWestReport(
            IGenericRepository<KenoWestOnesEntity> kenoWestOnesRepository,
            IGenericRepository<KenoWestRoundEntity> kenoWestRoundRepository,
            IGenericRepository<KenoWestBetEntity> kenoWestBetRepository,
            IGameUserService accountService)
        {
            this.kenoWestOnesRepository = kenoWestOnesRepository;
            this.kenoWestRoundRepository = kenoWestRoundRepository;
            this.kenoWestBetRepository = kenoWestBetRepository;

            this.accountService = accountService;
        }

        public async Task<IEnumerable<KenoBetHistory>> GetBetHistoryAsync(long roundId, string nickname)
        {
            var bets = await kenoWestBetRepository.FilterByAsync(bet => bet.Round == roundId && bet.Name == nickname);

            var betLogs = bets
                .Select(bet => new KenoBetHistory(
                    bet.Round,
                    bet.Name,
                    Enumeration.FromValue<KenoBetChoice>(bet.Choice),
                    bet.Amount,
                    bet.MemberId,
                    bet.SiteId,
                    bet.Time))
                .ToList();

            return betLogs;
        }

        public async Task<IEnumerable<KenoBetRecord>> GetReportAsync(long roundId, string nickname, bool excludeUUS)
        {
            var bets = await kenoWestOnesRepository.FilterByAsync(x => x.Round == roundId);

            var betRecords = bets
                .Select(bet => new KenoBetRecord(bet.ConvertAmountToDictionary(), bet.Odds)
                {
                    UserId = bet.Uid,
                    Nickname = bet.Name,
                    BetWin = bet.Betwin,
                    Time = bet.Time
                })
                .ToList();

            foreach (var bet in betRecords)
            {
                var account = await accountService.GetAccountAsync(bet.UserId, nickname, excludeUUS);
                bet.Username = account?.MemberName;
            }

            return betRecords.Where(bet => bet.Username != null);
        }

        public async Task<KenoRoundResult> GetRoundResultAsync(long roundId)
        {
            var round = await kenoWestRoundRepository.FindOneAsync(round => round.Number == roundId);

            return round == null || round.Result == null || round.SettlementResult == null
                ? default
                : new KenoRoundResult(
                    round.Result.ToArray(),
                    new KenoSettlementResult(round.SettlementResult),
                    round.Time);
        }
    }
}
