using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IThreeCardsReportRepository
    {
        Task<(ThreeCardsRoundEntity, IEnumerable<ThreeCardsBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS);
    }

    public class ThreeCardsReportRepository : BaseBetReportRepository, IThreeCardsReportRepository
    {
        public ThreeCardsReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(ThreeCardsRoundEntity, IEnumerable<ThreeCardsBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeUUS)
        {
            var roundResult = await database.GetCollection<ThreeCardsRoundEntity>(Document.GetCollectionName(typeof(ThreeCardsRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<ThreeCardsBetEntity, ThreeCardsBetWithUnwindAccountsEntity>(roundId, nickname, excludeUUS: excludeUUS);

                var records = (await project.ToListAsync())
                    .Select(x => new ThreeCardsBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        Red = x.Red,
                        Black = x.Black,
                        Diamonds = x.Diamonds,
                        Hearts = x.Hearts,
                        Spades = x.Spades,
                        Clubs = x.Clubs,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.PartnerId
                    });

                return (roundResult, records);
            }

            return (new ThreeCardsRoundEntity(), Enumerable.Empty<ThreeCardsBetReportEntity>());
        }
    }
}