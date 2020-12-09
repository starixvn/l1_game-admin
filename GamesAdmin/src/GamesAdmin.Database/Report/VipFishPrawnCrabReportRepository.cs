using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IVipFishPrawnCrabReportRepository
    {
        Task<(VipFishPrawnCrabRoundEntity, IEnumerable<VipFishPrawnCrabBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS);
    }
    public class VipFishPrawnCrabReportRepository : BaseBetReportRepository, IVipFishPrawnCrabReportRepository
    {
        public VipFishPrawnCrabReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(VipFishPrawnCrabRoundEntity, IEnumerable<VipFishPrawnCrabBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var roundResult = await database.GetCollection<VipFishPrawnCrabRoundEntity>(Document.GetCollectionName(typeof(VipFishPrawnCrabRoundEntity)))
                .Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<VipFishPrawnCrabBetEntity, VipFishPrawnCrabBetWithUnwindAccountsEntity>(roundId, nickname, excludeUUS: excludeUUS)
                    .Project(x => new VipFishPrawnCrabBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        Stag = x.Stag,
                        Gourd = x.Gourd,
                        Crab = x.Crab,
                        Fish = x.Fish,
                        Prawn = x.Prawn,
                        Rooster = x.Rooster,
                        AnyTriple = x.AnyTriple,
                        Mountain = x.Mountain,
                        Ocean = x.Ocean
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new VipFishPrawnCrabRoundEntity(), Enumerable.Empty<VipFishPrawnCrabBetReportEntity>());
        }
    }
}