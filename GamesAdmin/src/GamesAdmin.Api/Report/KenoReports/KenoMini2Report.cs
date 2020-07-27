using GamesAdmin.Api.Accounts;
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
    public class KenoMini2Report : IKenoReport
    {
        private readonly IGenericRepository<KenoMini2OnesEntity> kenoMini2OnesRepository;
        private readonly IGenericRepository<KenoMini2RoundEntity> kenoMini2RoundRepository;
        private readonly IGenericRepository<KenoMini2BetEntity> kenoMini2BetRepository;
        private readonly IAccountService accountService;

        public KenoMini2Report(
            IGenericRepository<KenoMini2OnesEntity> kenoMini2OnesRepository,
            IGenericRepository<KenoMini2RoundEntity> kenoMini2RoundRepository,
            IGenericRepository<KenoMini2BetEntity> kenoMini2BetRepository,
            IAccountService accountService)
        {
            this.kenoMini2OnesRepository = kenoMini2OnesRepository;
            this.kenoMini2RoundRepository = kenoMini2RoundRepository;
            this.kenoMini2BetRepository = kenoMini2BetRepository;

            this.accountService = accountService;
        }

        public async Task<IEnumerable<KenoBetHistory>> GetBetHistoryAsync(long roundId, string nickname)
        {
            var bets = await kenoMini2BetRepository.FilterByAsync(bet => bet.Round == roundId && bet.Name == nickname);

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
            var bets = await kenoMini2OnesRepository.FilterByAsync(x => x.Round == roundId);

            var betRecords = bets
                .Select(bet => new KenoBetRecord(bet.ConvertAmountToDictionary(), bet.Odds)
                {
                    UserId = bet.Uid.ToString(),
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
            var round = await kenoMini2RoundRepository.FindOneAsync(round => round.Number == roundId);

            return round == null || round.Result == null || round.SettlementResult == null
                ? default
                : new KenoRoundResult(
                    round.Result.ToArray(),
                    new KenoSettlementResult(round.SettlementResult),
                    round.Time);
        }
    }
}
