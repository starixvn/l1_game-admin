using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration.BetChoices;
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

        public async Task<KenoBetReport> GetReportAsync(long roundId, string nickname)
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
                .Select(bet => new KenoBetRecord(ConvertToAmountByChoice(bet), bet.Odds)
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

            return round == null
                ? default
                : new KenoRoundResult(round.Result.ToArray(), new KenoSettlementResult(round.SettlementResult));
        }

        private static Dictionary<string, decimal> ConvertToAmountByChoice(KenoProVietOnesEntity entity)
        => new Dictionary<string, decimal>
            {
                { KenoBetChoice.BigValue,  entity.Big},
                { KenoBetChoice.SmallValue,  entity.Small},
                { KenoBetChoice.OddValue,  entity.Odd},
                { KenoBetChoice.EvenValue,  entity.Even},
                { KenoBetChoice.GoldValue,  entity.Gold},
                { KenoBetChoice.EarthValue,  entity.Earth},
                { KenoBetChoice.FireValue,  entity.Fire},
                { KenoBetChoice.WaterValue,  entity.Water},
                { KenoBetChoice.WoodValue,  entity.Wood},
            };
    }
}
