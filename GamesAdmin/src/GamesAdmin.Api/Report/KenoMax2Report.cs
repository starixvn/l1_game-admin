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
    public class KenoMax2Report : IKenoReport
    {
        private readonly IGenericRepository<KenoMax2OnesEntity> kenoMax2OnesRepository;
        private readonly IGenericRepository<KenoMax2RoundEntity> kenoMax2RoundRepository;
        private readonly IGenericRepository<KenoMax2BetEntity> kenoMax2BetRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;

        public KenoMax2Report(
            IGenericRepository<KenoMax2OnesEntity> kenoMax2OnesRepository,
            IGenericRepository<KenoMax2RoundEntity> kenoMax2RoundRepository,
            IGenericRepository<KenoMax2BetEntity> kenoMax2BetRepository,
            IGenericRepository<AccountEntity> accountRepository)
        {
            this.kenoMax2OnesRepository = kenoMax2OnesRepository;
            this.kenoMax2RoundRepository = kenoMax2RoundRepository;
            this.kenoMax2BetRepository = kenoMax2BetRepository;

            this.accountRepository = accountRepository;
        }

        public async Task<IEnumerable<KenoBetHistory>> GetBetHistoryAsync(long roundId, string nickname)
        {
            var bets = await kenoMax2BetRepository.FilterByAsync(bet => bet.Round == roundId && bet.Name == nickname);

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
            var bets = await kenoMax2OnesRepository
                  .Aggregate()
                  .Match(x => x.Round == roundId)
                  .AppendStage<KenoMax2OnesEntity>("{$addFields: {userId: {$toObjectId: '$uid'}}}")
                  .Lookup<KenoMax2OnesEntity, AccountEntity, JoinedKenoBetOnes>(
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
            var round = await kenoMax2RoundRepository.FindOneAsync(round => round.Number == roundId);

            return round == null || round.Result == null || round.SettlementResult == null
                ? default
                : new KenoRoundResult(
                    round.Result.ToArray(),
                    new KenoSettlementResult(round.SettlementResult),
                    round.Time);
        }
    }
}
