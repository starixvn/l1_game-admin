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
    public interface ICascadingSlotGameReportRepository
    {
        Task<IEnumerable<CascadingSlotBetReportEntity>> GetRecordAsync(CascadingSlotReportFilter filter);

        Task<CascadingSlotBetHistoryEntity> GetBetHistoryAsync(CascadingSlotReportFilter filter);
    }

    public class CascadingSlotGameReportRepository : ICascadingSlotGameReportRepository
    {
        private readonly IMongoDatabase database;

        public CascadingSlotGameReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<CascadingSlotBetHistoryEntity> GetBetHistoryAsync(CascadingSlotReportFilter filter)
        {
            var round = await database
                .GetCollection<CascadingSlotRoundEntity>(NamingConverter.RoundCollection(filter.GameInfo.DisplayName)).Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

            var totalMultiplier = await database
                .GetCollection<CascadingSlotTotalMultiplierEntity>(NamingConverter.MultipliersCollection(filter.GameInfo.DisplayName)).Find(x => x.Name == filter.Nickname).ToListAsync();

            if (totalMultiplier.Count > 0)
            {
                var multiplier = totalMultiplier.FirstOrDefault(e => e.Rounds.Any(x => x.RoundId == filter.RoundId));
                if (multiplier != null)
                {
                    return new CascadingSlotBetHistoryEntity(round, multiplier.Rounds);
                }
            }

            return new CascadingSlotBetHistoryEntity(round);
        }

        public async Task<IEnumerable<CascadingSlotBetReportEntity>> GetRecordAsync(CascadingSlotReportFilter filter)
        {
            var betCollection = database.GetCollection<CascadingSlotRoundEntity>(NamingConverter.RoundCollection(filter.GameInfo.DisplayName));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));

            var bets = await betCollection.Find(GetFilterExpression(filter)).ToListAsync();

            var result = new List<CascadingSlotBetReportEntity>();
            var userNameRegex = new Regex($"^(?i){filter.Username}$");

            foreach (var bet in bets)
            {
                var betUser = await accountCollection.Find(acount => acount.Id == ObjectId.Parse(bet.Uid)).FirstOrDefaultAsync();

                if (string.IsNullOrWhiteSpace(filter.Username) || userNameRegex.IsMatch(betUser.Local.MemberName))
                {
                    result.Add(new CascadingSlotBetReportEntity
                    {
                        Time = bet.Time,
                        Username = betUser.Local.MemberName ?? betUser.Local.Username,
                        Nickname = bet.Name,
                        Betwin = bet.Betwin,
                        TotalAmount = bet.TotalAmount,
                        FreeSpin = bet.FreeSpin,
                        BuyFreeSpin = bet.BuyFreeSpin,
                        Number = bet.Number,
                        UserId = bet.Uid,
                        Paid = bet.Paid,
                        Currency = bet.Currency,
                        OperatorId = bet.OperatorId,
                        Rate = bet.Rate,
                        ExchangeRateId = bet.ExchangeRateId
                    });
                }
            }
            return result;
        }

        private Expression<Func<CascadingSlotRoundEntity, bool>> GetFilterExpression(CascadingSlotReportFilter filter)
        {
            Expression<Func<CascadingSlotRoundEntity, bool>> expression;
            if (filter.RoundId > 0)
            {
                expression = x => x.Number == filter.RoundId;
            }
            else
            {
                var from = filter.FromDate.ToStartDate().ToGMT7();
                var to = filter.ToDate.ToEndDate().ToGMT7();
                expression = x => from <= x.Time && x.Time <= to;
            }

            if (!string.IsNullOrWhiteSpace(filter.PartnerId))
            {
                expression = expression.And(x => x.OperatorId == filter.PartnerId);
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

            return expression;
        }
    }
}