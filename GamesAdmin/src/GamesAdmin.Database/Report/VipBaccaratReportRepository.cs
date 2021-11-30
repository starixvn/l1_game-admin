using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IVipBaccaratReportRepository
    {
        Task<(VipBaccaratRoundEntity, IEnumerable<BaccaratBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS);
    }

    public class VipBaccaratReportRepository : BaseBetReportRepository, IVipBaccaratReportRepository
    {
        public VipBaccaratReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(VipBaccaratRoundEntity, IEnumerable<BaccaratBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var roundResult = await database.GetCollection<VipBaccaratRoundEntity>(Document.GetCollectionName(typeof(VipBaccaratRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<VipBaccaratBetEntity, VipBaccaratBetWithUnWindAccountsEntity>(roundId, nickname, excludeUUS: excludeUUS)
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
                        Tie = x.Tie,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.PartnerId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new VipBaccaratRoundEntity(), Enumerable.Empty<BaccaratBetReportEntity>());
        }
    }
}