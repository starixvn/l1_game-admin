using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities.Keno;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IMiniKenoReportRepository
    {
        Task<(TRoundEntity, IEnumerable<MiniKenoBetReportEntity>)> GetAsync<TRoundEntity, TOneEntity>(BetReportFilter filter)
                where TRoundEntity : BaseKenoRoundEntity, new()
                where TOneEntity : BaseKenoOnesEntity;
    }

    public class MiniKenoReportRepository : BaseBetReportRepository, IMiniKenoReportRepository

    {
        public MiniKenoReportRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<(TRoundEntity, IEnumerable<MiniKenoBetReportEntity>)> GetAsync<TRoundEntity, TOneEntity>(BetReportFilter filter)
            where TRoundEntity : BaseKenoRoundEntity, new()
            where TOneEntity : BaseKenoOnesEntity
        {
            var roundResult = await database.GetCollection<TRoundEntity>(Document.GetCollectionName(typeof(TRoundEntity)))
                .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            if (roundResult != null)
            {
                var project = GetBaseAggregateBuilder<TOneEntity, MiniKenoBetWithUnWindAccountsEntity>(
                    filter.RoundId, filter.Nickname, filter.ExcludeBot, filter.ExcludeUUS, filter.PartnerId, filter.Username)
                    .Project(x => new MiniKenoBetReportEntity
                    {
                        Time = x.Time,
                        Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                        Nickname = x.AccountInfos.Name,
                        BetWin = x.Betwin,
                        Big = x.Big,
                        Small = x.Small,
                        Odd = x.Odd,
                        Even = x.Even,
                        Earth = x.Earth,
                        Fire = x.Fire,
                        Gold = x.Gold,
                        Water = x.Water,
                        Wood = x.Wood,
                        Odds = x.Odds,
                        UserId = x.Uid,
                        Currency = x.Currency,
                        Rate = x.Rate,
                        PartnerId = x.PartnerId
                    });
                var records = await project.ToListAsync();

                return (roundResult, records);
            }

            return (new TRoundEntity(), Enumerable.Empty<MiniKenoBetReportEntity>());
        }
    }
}