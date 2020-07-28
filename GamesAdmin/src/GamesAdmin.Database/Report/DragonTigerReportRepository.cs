using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IDragonTigerReportRepository
    {
        Task<(DragonTigerRoundEntity, IEnumerable<DragonTigerBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }
    public class DragonTigerReportRepository : BaseBetReportRepository, IDragonTigerReportRepository
    {
        public DragonTigerReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(DragonTigerRoundEntity, IEnumerable<DragonTigerBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            var roundResult = await database.GetCollection<DragonTigerRoundEntity>(Document.GetCollectionName(typeof(DragonTigerRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<DragonTigerBetEntity, DragonTigerBetWithUnWindAccountsEntity>(roundId, nickname, excludeBot)
                    .Project(x => new DragonTigerBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        Dragon = x.Dragon,
                        Tiger = x.Tiger,
                        Tie = x.Tie
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new DragonTigerRoundEntity(), Enumerable.Empty<DragonTigerBetReportEntity>());
        }
    }
}