using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Keno;
using MongoDB.Driver;

namespace GamesAdmin.Api.Report
{
    public class KenoMaxReport : IKenoReport
    {

        private readonly IGenericRepository<KenoMaxOnesEntity> kenoMaxOnesRepository;
        private readonly IGenericRepository<KenoMaxRoundEntity> kenoMaxRoundRepository;
        private readonly IGenericRepository<KenoMaxBetEntity> kenoMaxBetRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;

        public KenoMaxReport(
            IGenericRepository<KenoMaxOnesEntity> kenoMaxOnesRepository,
            IGenericRepository<KenoMaxRoundEntity> kenoMaxRoundRepository,
            IGenericRepository<KenoMaxBetEntity> kenoMaxBetRepository,
            IGenericRepository<AccountEntity> accountRepository)
        {
            this.kenoMaxOnesRepository = kenoMaxOnesRepository;
            this.kenoMaxRoundRepository = kenoMaxRoundRepository;
            this.kenoMaxBetRepository = kenoMaxBetRepository;

            this.accountRepository = accountRepository;
        }

        public async Task<IEnumerable<KenoBetHistory>> GetBetHistoryAsync(long roundId, string nickname)
        {
            var bets = await kenoMaxBetRepository.FilterByAsync(bet => bet.Round == roundId && bet.Name == nickname);

            var betLogs = bets
                .Select(bet => new KenoBetHistory(
                    bet.Round,
                    bet.Name,
                    Enumeration.FromValue<KenoBetChoice>(bet.Choice),
                    bet.Amount,
                    bet.MemberId,
                    bet.SiteId,
                    new DateTimeOffset(bet.Time)))
                .ToList();

            return betLogs;
        }

        public async Task<IEnumerable<KenoBetRecord>> GetReportAsync(long roundId)
        {
            var bets = await kenoMaxOnesRepository
                  .Aggregate()
                  .Match(x => x.Round == roundId)
                  .AppendStage<KenoProVietOnesEntity>("{$addFields: {userId: {$toObjectId: '$uid'}}}")
                  .Lookup<KenoProVietOnesEntity, AccountEntity, JoinedKenoBetOnes>(
                      accountRepository.Collection(),
                      x => x.UserId,
                      y => y.Id,
                      y => y.InnerUsers
                  )
                  .ToListAsync();

            var betRecords = bets
                .Select(bet => new KenoBetRecord(bet.ConvertAmountToDictionary(), bet.Odds)
                {
                    Nickname = bet.Name,
                    BetWin = bet.Betwin,
                    Time = bet.Time,
                    Username = bet.InnerUsers.FirstOrDefault().Local.Username
                })
                .ToList();

            return betRecords;
        }

        public async Task<KenoRoundResult> GetRoundResultAsync(long roundId)
        {
            var round = await kenoMaxRoundRepository.FindOneAsync(round => round.Number == roundId);

            return round == null || round.Result == null || round.SettlementResult == null
                ? default
                : new KenoRoundResult(
                    round.Result.ToArray(),
                    new KenoSettlementResult(round.SettlementResult),
                    round.Time);
        }
    }
}
