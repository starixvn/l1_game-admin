using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface ISlotGameReportRepository
    {
        Task<IEnumerable<SlotGameBetReportEntity>> GetAsync(SlotGameBetReportFilter filter);
    }

    public class SlotGameReportRepository : ISlotGameReportRepository
    {
        private readonly IMongoDatabase database;

        public SlotGameReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<SlotGameBetReportEntity>> GetAsync(SlotGameBetReportFilter filter)
        {
            var userNameRegex = new Regex($"^(?i){filter.Username}$");

            var betCollection = database.GetCollection<SlotGameRoundEntity>(NamingConverter.RoundCollection(filter.GameInfo.DisplayName));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));

            var bets = await betCollection.Find(GetFilterExpression(filter)).ToListAsync();

            var result = new List<SlotGameBetReportEntity>();

            foreach (var bet in bets)
            {
                var betUser = await accountCollection.Find(acount => acount.Id == ObjectId.Parse(bet.Uid)).FirstOrDefaultAsync();

                if (string.IsNullOrWhiteSpace(filter.Username) || userNameRegex.IsMatch(betUser.Local.MemberName))
                {
                    result.Add(new SlotGameBetReportEntity
                    {
                        Time = bet.Time,
                        Username = betUser.Local.MemberName ?? betUser.Local.Username,
                        Nickname = bet.Name,
                        Bet = bet.Bet,
                        BetWin = bet.BetWin,
                        TotalAmount = bet.TotalAmount,
                        BonusChests = bet.BonusChests,
                        FreeSpin = bet.FreeSpin,
                        Number = bet.Number,
                        Paylines = bet.Paylines,
                        SettlementResult = bet.SettlementResult,
                        Result = bet.Result,
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

        private static Expression<Func<SlotGameRoundEntity, bool>> GetFilterExpression(SlotGameBetReportFilter filter)
        {
            Expression<Func<SlotGameRoundEntity, bool>> expression;
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