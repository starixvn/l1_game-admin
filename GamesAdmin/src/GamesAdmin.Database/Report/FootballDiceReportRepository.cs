using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IFootballDiceReportRepository
    {
        Task<(BaseRoundEntity, IEnumerable<BetReportEntity>)> GetAsync(BetReportFilter filter);
    }

    public class FootballDiceReportRepository : BaseBetReportRepository, IFootballDiceReportRepository
    {
        public FootballDiceReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(BaseRoundEntity, IEnumerable<BetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<FootballDiceRoundEntity>(Document.GetCollectionName(typeof(FootballDiceRoundEntity))).Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<FootballDiceBetEntity, FootballDiceBetWithUnWindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username);

                var records = (await project.ToListAsync())
                    .Select(x => new FootballDiceBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        Home = x.Home,
                        Away = x.Away,
                        Draw = x.Draw,
                        FreeBet = x.FreeBet ?? false,
                        Currency = x.Currency,
                        PartnerId = x.OperatorId,
                        Rate = x.Rate
                    });

                return (roundResult, records);
            }

            return (new FootballDiceRoundEntity(), Enumerable.Empty<FootballDiceBetReportEntity>());
        }
    }
}