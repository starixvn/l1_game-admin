using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesAdmin.Database
{
    public interface IMiniRouletteReportRepository
    {
        Task<(MiniRouletteRoundEntity, IEnumerable<MiniRouletteBetReportEntity>)> GetAsync(BetReportFilter filter);
    }

    public class MiniRouletteReportRepository : BaseBetReportRepository, IMiniRouletteReportRepository
    {
        public MiniRouletteReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(MiniRouletteRoundEntity, IEnumerable<MiniRouletteBetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<MiniRouletteRoundEntity>(Document.GetCollectionName(typeof(MiniRouletteRoundEntity))).Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<MiniRouletteBetEntity, MiniRouletteBetWithUnWindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new MiniRouletteBetReportEntity
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

            return (new MiniRouletteRoundEntity(), Enumerable.Empty<MiniRouletteBetReportEntity>());
        }
    }
}
