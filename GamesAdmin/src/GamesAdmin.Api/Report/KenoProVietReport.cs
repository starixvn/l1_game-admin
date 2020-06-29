using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Keno;
using MongoDB.Driver;

namespace GamesAdmin.Api.Report
{
    public class KenoProVietReport : IKenoReport
    {
        private readonly IGenericRepository<KenoProVietOnesEntity> kenoProVietOnesRepository;
        private readonly IGenericRepository<KenoRoundEntity> kenoProvietRoundRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;

        public KenoProVietReport(
            IGenericRepository<KenoProVietOnesEntity> kenoProVietOnesRepository,
            IGenericRepository<KenoRoundEntity> kenoProvietRoundRepository,
            IGenericRepository<AccountEntity> accountRepository)
        {
            this.kenoProVietOnesRepository = kenoProVietOnesRepository;
            this.kenoProvietRoundRepository = kenoProvietRoundRepository;
            this.accountRepository = accountRepository;
        }

        public async Task<KenoBetReport> GetReportAsync(long roundId)
        {
            var roundResult = await GetKenoRoundAsync(roundId);

            var newBets = await kenoProVietOnesRepository
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

            var betRecords = newBets
                .Select(bet => new KenoBetRecord(bet.ConvertAmountToDictionary(), bet.Odds)
                {
                    Nickname = bet.Name,
                    BetWin = bet.Betwin,
                    Time = bet.Time,
                    Username = bet.InnerUsers.FirstOrDefault().Local.Username
                })
                .ToList();

            return new KenoBetReport(roundResult, betRecords);
        }

        private async Task<KenoRoundResult> GetKenoRoundAsync(long roundId)
        {
            var round = await kenoProvietRoundRepository.FindOneAsync(round => round.Number == roundId);

            return round == null || round.Result == null || round.SettlementResult == null
                ? default
                : new KenoRoundResult(round.Result.ToArray(), new KenoSettlementResult(round.SettlementResult));
        }
    }
}
