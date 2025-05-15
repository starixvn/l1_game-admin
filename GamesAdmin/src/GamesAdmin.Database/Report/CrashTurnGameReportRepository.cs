using System;
using System.Collections.Generic;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Report.Entities;
using System.Threading.Tasks;
using MongoDB.Driver;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Linq;

namespace GamesAdmin.Database.Report
{
    public interface ICrashTurnGameReportRepository
    {

        Task<IEnumerable<CrashTurnGameBetReportEntity>> GetRoundHistoryAsync(CrashTurnGameBetReportFilter filter);
    }

    public class CrashTurnGameReportRepository : ICrashTurnGameReportRepository
    {
        private readonly IMongoDatabase database;

        public CrashTurnGameReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        private Expression<Func<CrashTurnGameRoundEntity, bool>> GetRoundFilterExpression(CrashTurnGameBetReportFilter filter)
        {
            Expression<Func<CrashTurnGameRoundEntity, bool>> expression;

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


        private Expression<Func<CrashTurnGameBetLogEntity, bool>> GetFilterBetLogExpression(CrashTurnGameBetReportFilter filter)
        {
            Expression<Func<CrashTurnGameBetLogEntity, bool>> expression;
            if (filter.RoundId > 0)
            {
                expression = x => x.Round == filter.RoundId;
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

        public async Task<IEnumerable<CrashTurnGameBetReportEntity>> GetRoundHistoryAsync(CrashTurnGameBetReportFilter filter)
        {

            var roundCollection = database.GetCollection<CrashTurnGameRoundEntity>(NamingConverter.RoundCollection(filter.GameInfo.DisplayName));
            var betLogCollection = database.GetCollection<CrashTurnGameBetLogEntity>(NamingConverter.BetLogCollection(filter.GameInfo.DisplayName));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var rounds = await roundCollection.Find(GetRoundFilterExpression(filter)).ToListAsync();
            if (!rounds.Any())
            {
                return Enumerable.Empty<CrashTurnGameBetReportEntity>();
            }
            var result = new List<CrashTurnGameBetReportEntity>();

            var betLogs = await betLogCollection.Find(GetFilterBetLogExpression(filter)).ToListAsync();
            var userNameRegex = new Regex($"^(?i){filter.Username}$");

            foreach (var round in rounds)
            {
                var betUser = await accountCollection.Find(account => account.Id == ObjectId.Parse(round.Uid)).FirstOrDefaultAsync();
                if (string.IsNullOrWhiteSpace(filter.Username) || userNameRegex.IsMatch(betUser.Local.MemberName))
                {
                    result.Add(new CrashTurnGameBetReportEntity
                    {
                        Time = round.Time,
                        Username = betUser.Local.MemberName ?? betUser.Local.Username,
                        Nickname = round.Name,
                        UserId = round.Uid,
                        Rate = betLogs.First(x => x.Round == round.Number).Rate,
                        Amount = round.Amount,
                        BetWin = round.Betwin,
                        Currency = round.Currency,
                        Round = round.Number,
                        PartnerId = round.OperatorId,
                        Result=round.Result,
                        ExchangeRateId= round.ExchangeRateId,
                        BetLogs = betLogs.Where(x => x.Round == round.Number),
                    });

                }
            }

            return result;
        }
    }
}
