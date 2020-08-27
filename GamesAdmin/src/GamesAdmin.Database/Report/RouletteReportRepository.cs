using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IRouletteReportRepository
    {
        Task<(RouletteRoundEntity, IEnumerable<RouletteBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS);
    }

    public class RouletteReportRepository : BaseBetReportRepository, IRouletteReportRepository
    {
        public RouletteReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(RouletteRoundEntity, IEnumerable<RouletteBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var roundResult = await database.GetCollection<RouletteRoundEntity>(Document.GetCollectionName(typeof(RouletteRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<RouletteOnesEntity, RouletteBetWithUnWindAccountsEntity>(roundId, nickname, excludeUUS: excludeUUS)
                    .Project(x => new RouletteBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        UserId = x.Uid,
                        BetWin = x.Betwin,
                        TotalBet = x.TotalAmount
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new RouletteRoundEntity(), Enumerable.Empty<RouletteBetReportEntity>());
        }
    }
}