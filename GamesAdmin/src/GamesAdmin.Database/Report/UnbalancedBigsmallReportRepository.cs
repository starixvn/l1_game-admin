using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IUnbalancedBigSmallReportRepository
    {
        Task<(TRoundEntity, IEnumerable<UnbalancedBigSmallBetReportEntity>)> GetAsync<TRoundEntity, TBetEntity>(BetReportFilter filter)
                where TRoundEntity : UnbalancedBigSmallRoundEntity, new()
                where TBetEntity : UnbalancedBigSmallBetEntity;
    }

    public class UnbalancedBigSmallReportRepository : BaseBetReportRepository, IUnbalancedBigSmallReportRepository

    {
        public UnbalancedBigSmallReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(TRoundEntity, IEnumerable<UnbalancedBigSmallBetReportEntity>)> GetAsync<TRoundEntity, TBetEntity>(BetReportFilter filter)
            where TRoundEntity : UnbalancedBigSmallRoundEntity, new()
            where TBetEntity : UnbalancedBigSmallBetEntity
        {
            var roundResult = await database.GetCollection<TRoundEntity>(Document.GetCollectionName(typeof(TRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<TBetEntity, UnbalancedBigSmallBetWithUnwindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new UnbalancedBigSmallBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        TotalBet = x.TotalAmount,
                        UserId = x.Uid,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.OperatorId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new TRoundEntity(), Enumerable.Empty<UnbalancedBigSmallBetReportEntity>());
        }
    }
}