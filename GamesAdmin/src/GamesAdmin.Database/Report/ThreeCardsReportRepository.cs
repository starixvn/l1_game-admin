using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IThreeCardsReportRepository
    {
        Task<(ThreeCardsRoundEntity, IEnumerable<ThreeCardsBetReportEntity>)> GetAsync(BetReportFilter filter);
    }

    public class ThreeCardsReportRepository : BaseBetReportRepository, IThreeCardsReportRepository
    {
        public ThreeCardsReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(ThreeCardsRoundEntity, IEnumerable<ThreeCardsBetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<ThreeCardsRoundEntity>(Document.GetCollectionName(typeof(ThreeCardsRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<ThreeCardsBetEntity, ThreeCardsBetWithUnwindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username);

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
                        PartnerId = x.OperatorId
                    });

                return (roundResult, records);
            }

            return (new ThreeCardsRoundEntity(), Enumerable.Empty<ThreeCardsBetReportEntity>());
        }
    }
}