using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IRouletteReportRepository
    {
        Task<(RouletteRoundEntity, IEnumerable<RouletteBetReportEntity>)> GetAsync(BetReportFilter filter);
    }

    public class RouletteReportRepository : BaseBetReportRepository, IRouletteReportRepository
    {
        public RouletteReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(RouletteRoundEntity, IEnumerable<RouletteBetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<RouletteRoundEntity>(Document.GetCollectionName(typeof(RouletteRoundEntity))).Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<RouletteOnesEntity, RouletteBetWithUnWindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new RouletteBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        UserId = x.Uid,
                        BetWin = x.Betwin,
                        TotalBet = x.TotalAmount,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.OperatorId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new RouletteRoundEntity(), Enumerable.Empty<RouletteBetReportEntity>());
        }
    }
}