using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IBitcoinMasterReportRepository
    {
        Task<(BitcoinMasterRoundEntity, IEnumerable<BitcoinMasterBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }

    public class BitcoinMasterReportRepository : BaseBetReportRepository, IBitcoinMasterReportRepository
    {
        public BitcoinMasterReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BitcoinMasterRoundEntity, IEnumerable<BitcoinMasterBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            var roundResult = await database.GetCollection<BitcoinMasterRoundEntity>(Document.GetCollectionName(typeof(BitcoinMasterRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult == null)
            {
                return (new BitcoinMasterRoundEntity(), Enumerable.Empty<BitcoinMasterBetReportEntity>());
            }

            var project = GetBaseAggregateBuilder<BitcoinMasterBetEntity, BitcoinMasterBetWithUnwindAccountsEntity>(roundId, nickname, excludeBot, true)
                .Project(x => new BitcoinMasterBetReportEntity
                {
                    Time = x.Time,
                    Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                    Nickname = x.AccountInfos.Name,
                    Bet = x.Bet ?? x.Amount,
                    BetWin = x.Betwin,
                    Back = x.Back ?? x.Refund
                });
            var records = await project.ToListAsync();

            return (roundResult, records);
        }
    }
}