using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IBlockchainBaccaratReportRepository
    {
        Task<(BlockchainBaccaratRoundEntity, IEnumerable<BaccaratBetReportEntity>)> GetAsync(BetReportFilter filter);
    }

    public class BlockchainBaccaratReportRepository : BaseBetReportRepository, IBlockchainBaccaratReportRepository
    {
        public BlockchainBaccaratReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BlockchainBaccaratRoundEntity, IEnumerable<BaccaratBetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<BlockchainBaccaratRoundEntity>(Document.GetCollectionName(
                typeof(BlockchainBaccaratRoundEntity))).Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<BlockchainBaccaratBetEntity, BlockchainBaccaratBetWithUnWindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
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

            return (new BlockchainBaccaratRoundEntity(), Enumerable.Empty<BaccaratBetReportEntity>());
        }
    }
}