using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IVipFishPrawnCrabReportRepository
    {
        Task<(VipFishPrawnCrabRoundEntity, IEnumerable<VipFishPrawnCrabBetReportEntity>)> GetAsync(BetReportFilter filter);
    }
    public class VipFishPrawnCrabReportRepository : BaseBetReportRepository, IVipFishPrawnCrabReportRepository
    {
        public VipFishPrawnCrabReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(VipFishPrawnCrabRoundEntity, IEnumerable<VipFishPrawnCrabBetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<VipFishPrawnCrabRoundEntity>(Document.GetCollectionName(typeof(VipFishPrawnCrabRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<VipFishPrawnCrabBetEntity, VipFishPrawnCrabBetWithUnwindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
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
                        Ocean = x.Ocean,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.PartnerId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new VipFishPrawnCrabRoundEntity(), Enumerable.Empty<VipFishPrawnCrabBetReportEntity>());
        }
    }
}