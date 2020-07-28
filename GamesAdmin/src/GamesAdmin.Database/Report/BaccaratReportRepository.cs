using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Baccarat;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IBaccaratReportRepository
    {
        Task<(BaccaratRoundEntity, IEnumerable<BaccaratBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }
    public class BaccaratReportRepository : BaseBetReportRepository, IBaccaratReportRepository
    {
        public BaccaratReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BaccaratRoundEntity, IEnumerable<BaccaratBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            var roundResult = await database.GetCollection<BaccaratRoundEntity>(Document.GetCollectionName(typeof(BaccaratRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<BaccaratBetEntity, BaccaratBetWithUnWindAccountsEntity>(roundId, nickname, excludeBot)
                    .Project(x => new BaccaratBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        Banker = x.Banker,
                        BankerPair = x.BankerPair,
                        Big = x.Big,
                        Small = x.Small,
                        PlayerPair = x.PlayerPair,
                        Player = x.Player,
                        Tie = x.Tie
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new BaccaratRoundEntity(), Enumerable.Empty<BaccaratBetReportEntity>());
        }
    }
}