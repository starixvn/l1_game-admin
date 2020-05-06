using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IBigSmallTurboReportRepository
    {
        Task<(BigSmallTurboRoundEntity, IEnumerable<BigSmallBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }

    public class BigSmallTurboReportRepository : BaseBetReportRepository, IBigSmallTurboReportRepository
    {
        public BigSmallTurboReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BigSmallTurboRoundEntity, IEnumerable<BigSmallBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            var roundResult = await database.GetCollection<BigSmallTurboRoundEntity>(Document.GetCollectionName(typeof(BigSmallTurboRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var records = await GetBaseAggregateBuilder<BigSmallTurboBetEntity, BigSmallBetWithUnwindAccountsEntity>(roundId, nickname, excludeBot)
                    .Project(x => new BigSmallBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        Select = x.Select,
                        Bet = x.Bet,
                        BetWin = x.Betwin,
                        Back = x.Back
                    }).ToListAsync();

                return (roundResult, records);
            }

            return (new BigSmallTurboRoundEntity(), Enumerable.Empty<BigSmallBetReportEntity>());
        }
    }
}