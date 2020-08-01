using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IFishPrawnCrabReportRepository
    {
        Task<(FishPrawnCrabRoundEntity, IEnumerable<FishPrawnCrabBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS);
    }
    public class FishPrawnCrabReportRepository : BaseBetReportRepository, IFishPrawnCrabReportRepository
    {
        public FishPrawnCrabReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(FishPrawnCrabRoundEntity, IEnumerable<FishPrawnCrabBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var roundResult = await database.GetCollection<FishPrawnCrabRoundEntity>(Document.GetCollectionName(typeof(FishPrawnCrabRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<FishPrawnCrabBetEntity, FishPrawnCrabBetWithUnwindAccountsEntity>(roundId, nickname, excludeUUS: excludeUUS)
                    .Project(x => new FishPrawnCrabBetReportEntity
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
                        Rooster = x.Rooster
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new FishPrawnCrabRoundEntity(), Enumerable.Empty<FishPrawnCrabBetReportEntity>());
        }
    }
}