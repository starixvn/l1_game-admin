using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface ISpecialSeDieReportRepository
    {
        Task<(SpecialSeDieRoundEntity, IEnumerable<SpecialSeDieBetReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot, string partnerId, string username);
    }

    public class SpecialSeDieReportRepository : BaseBetReportRepository, ISpecialSeDieReportRepository
    {
        public SpecialSeDieReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(SpecialSeDieRoundEntity, IEnumerable<SpecialSeDieBetReportEntity>)> GetAsync(
            long roundId,
            string nickname,
            bool excludeBot,
            string partnerId,
            string username)
        {
            var roundResult = await database.GetCollection<SpecialSeDieRoundEntity>(Document.GetCollectionName(typeof(SpecialSeDieRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<SpecialSeDieBetEntity, SpecialSeDieBetWithUnWindAccountsEntity>(roundId, nickname, excludeBot, true, partnerId, username)
                    .Project(x => new SpecialSeDieBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        Amount = x.Amount,
                        Choice = x.Choice,
                        Refund = x.Refund,
                        UserId = x.Uid,
                        Currency = x.Currency,
                        Partner = x.PartnerId,
                        Rate = x.Rate
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new SpecialSeDieRoundEntity(), Enumerable.Empty<SpecialSeDieBetReportEntity>());
        }
    }
}