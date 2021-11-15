using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Baccarat;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface ICasinoHoldemPokerReportRepository
    {
        Task<(CasinoHoldemPokerRoundEntity, IEnumerable<CasinoHoldemPokerBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS);
    }

    public class CasinoHoldemPokerReportRepository : BaseBetReportRepository, ICasinoHoldemPokerReportRepository
    {
        public CasinoHoldemPokerReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(CasinoHoldemPokerRoundEntity, IEnumerable<CasinoHoldemPokerBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var roundResult = await database.GetCollection<CasinoHoldemPokerRoundEntity>(Document.GetCollectionName(typeof(CasinoHoldemPokerRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<CasinoHoldemPokerBetEntity, CasinoHoldemPokerBetWithUnWindAccountsEntity>(roundId, nickname, excludeUUS: excludeUUS)
                    .Project(x => new CasinoHoldemPokerBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        AA = x.AA,
                        Ante = x.Ante,
                        Call = x.Call
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new CasinoHoldemPokerRoundEntity(), Enumerable.Empty<CasinoHoldemPokerBetReportEntity>());
        }
    }
}