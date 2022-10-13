using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IFanTanReportRepository
    {
        Task<(FanTanRoundEntity, IEnumerable<FanTanBetReportEntity>)> GetAsync(BetReportFilter filter);
    }

    public class FanTanReportRepository : BaseBetReportRepository, IFanTanReportRepository
    {
        public FanTanReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(FanTanRoundEntity, IEnumerable<FanTanBetReportEntity>)> GetAsync(BetReportFilter filter)
        {
            var roundResult = await database.GetCollection<FanTanRoundEntity>(Document.GetCollectionName(typeof(FanTanRoundEntity))).Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult == null)
            {
                return (new FanTanRoundEntity(), Enumerable.Empty<FanTanBetReportEntity>());
            }

            var project = GetBaseAggregateBuilder<FanTanBetEntity, FanTanBetWithUnWindAccountsEntity>(
                filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                .Project(x => new FanTanBetReportEntity
                {
                    Time = x.Time,
                    Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                    Nickname = x.AccountInfos.Name,
                    UserId = x.Uid,
                    BetWin = x.Betwin,
                    TotalBet = x.Odd + x.Even + x.Fan1 + x.Fan2 + x.Fan3 + x.Fan4 +
                        x.Kwok12 + x.Kwok23 + x.Kwok34 + x.Kwok41 +
                        x.Chun123 + x.Chun124 + x.Chun134 + x.Chun234,
                    Currency = x.Currency,
                    Rate = x.Rate,
                    PartnerId = x.OperatorId
                });

            var records = await project.ToListAsync();

            return (roundResult, records);
        }
    }
}