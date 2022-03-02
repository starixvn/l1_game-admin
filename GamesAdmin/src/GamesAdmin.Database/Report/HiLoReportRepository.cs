using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IHiLoReportRepository
    {
        Task<IEnumerable<HiLoBetReportEntity>> GetAsync(HiLoBetReportFilter filter);
    }

    public class HiLoReportRepository : IHiLoReportRepository
    {
        private readonly IMongoDatabase database;

        public HiLoReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<HiLoBetReportEntity>> GetAsync(HiLoBetReportFilter filter)
        {
            var rounds = await GetRounds(filter);

            if (rounds.Any())
            {
                var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
                var users = Enumerable.Empty<AccountEntity>();

                if (!string.IsNullOrWhiteSpace(filter.Username))
                {
                    var userNameRegex = new Regex($"^(?i){filter.Username}$");
                    users = await accountCollection.Find(x => userNameRegex.IsMatch(x.Local.MemberName)).ToListAsync();
                    rounds = rounds.Where(x => users.Any(u => u.Id.ToString() == x.Uid));
                }
                else
                {
                    var uids = rounds.Select(x => new ObjectId(x.Uid));
                    users = await accountCollection.Find(x => uids.Contains(x.Id)).ToListAsync();
                }

                return rounds.Select(x =>
                {
                    var user = users.FirstOrDefault(u => u.Id.ToString() == x.Uid);
                    return new HiLoBetReportEntity
                    {
                        Time = x.Time,
                        Username = user.Local.MemberName,
                        Nickname = x.Name,
                        BetWin = x.Betwin,
                        Amount = x.Amount,
                        Paid = x.Paid,
                        Currency = x.Currency,
                        PartnerId = x.PartnerId,
                        Rate = x.Rate,
                        RoundId = x.Number,
                        UserId = x.Uid,
                        ExchangeRateId = x.ExchangeRateId
                    };
                });
            }

            return Enumerable.Empty<HiLoBetReportEntity>();
        }

        private async Task<IEnumerable<HiLoRoundEntity>> GetRounds(HiLoBetReportFilter filter)
        {
            var hiloRoundCollection = database.GetCollection<HiLoRoundEntity>(Document.GetCollectionName(typeof(HiLoRoundEntity)));
            Expression<Func<HiLoRoundEntity, bool>> expression;

            if (filter.RoundId > 0)
            {
                expression = x => x.Number == filter.RoundId;
            }
            else
            {
                expression = x => x.Time <= filter.ToDate.ToUniversalTime() && x.Time >= filter.FromDate.ToUniversalTime();
            }

            if (!string.IsNullOrWhiteSpace(filter.PartnerId))
            {
                expression = expression.And(x => x.PartnerId == filter.PartnerId);
            }

            if (!string.IsNullOrWhiteSpace(filter.Nickname))
            {
                var nickNameRegex = new Regex($"^(?i){filter.Nickname}$");
                expression = expression.And(x => nickNameRegex.IsMatch(x.Name));
            }

            if (filter.ExcludeUUS)
            {
                expression = expression.And(x => x.Currency != "UUS");
            }

            return await hiloRoundCollection.Find(expression).ToListAsync();
        }
    }
}