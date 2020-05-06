using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IBigSmallReportRepository
    {
        Task<(BigSmallRoundEntity, IEnumerable<BigSmallBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }

    public class BigSmallReportRepository : BaseBetReportRepository, IBigSmallReportRepository
    {
        public BigSmallReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BigSmallRoundEntity, IEnumerable<BigSmallBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            var roundResult = await database.GetCollection<BigSmallRoundEntity>(Document.GetCollectionName(typeof(BigSmallRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<BigSmallBetEntity, BigSmallBetWithUnwindAccountsEntity>(roundId, nickname, excludeBot)
                    .Project(x => new BigSmallBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        Select = x.Select,
                        Bet = x.Bet,
                        BetWin = x.Betwin,
                        Back = x.Back
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new BigSmallRoundEntity(), Enumerable.Empty<BigSmallBetReportEntity>());
        }
    }
}