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
    public interface ICrashGameReportRepository
    {

        Task<(CrashGameRoundEntity, IEnumerable<CrashGameBetReportEntity>)> GetRecordAsync(SpeedUpGameBetReportFilter filter);

    }
    public class CrashGameReportRepository: ICrashGameReportRepository
    {
        private readonly IMongoDatabase database;

        public CrashGameReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<(CrashGameRoundEntity, IEnumerable<CrashGameBetReportEntity>)> GetRecordAsync(SpeedUpGameBetReportFilter filter)
        {
            try
            {
                var roundResult = await database.GetCollection<CrashGameRoundEntity>(NamingConverter.RoundCollection(filter.GameInfo.DisplayName))
                    .Find(x => x.Number == filter.RoundId).FirstOrDefaultAsync();

                if (roundResult == null)
                {
                    return (new CrashGameRoundEntity(), Enumerable.Empty<CrashGameBetReportEntity>());
                }

                var betCollection = database.GetCollection<CrashGameBetEntity>(NamingConverter.BetCollection(filter.GameInfo.DisplayName));
                var betLogCollection = database.GetCollection<CrashGameBetLogEntity>(NamingConverter.BetLogCollection(filter.GameInfo.DisplayName));
                var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
                var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));
                var bets = await betCollection.Find(GetFilterExpression(filter)).ToListAsync();    
                var betLogs = await betLogCollection.Find(GetFilterBetLogExpression(filter)).ToListAsync();
                var result = new List<CrashGameBetReportEntity>();
                var userNameRegex = new Regex($"^(?i){filter.Username}$");

                foreach (var bet in bets)
                {
                    var betUser = await accountCollection.Find(acount => acount.Id == ObjectId.Parse(bet.Uid)).FirstOrDefaultAsync();
                    if (string.IsNullOrWhiteSpace(filter.Username) || userNameRegex.IsMatch(betUser.Local.MemberName))
                    {
                        result.Add(new CrashGameBetReportEntity
                        {
                            Time = bet.Time,
                            Username = betUser.Local.MemberName ?? betUser.Local.Username,
                            Nickname = bet.Name,
                            UserId = bet.Uid,
                            Rate = bet.Rate,
                            Amount = bet.Amount,
                            BetWin = bet.Betwin,
                            Currency = bet.Currency,
                            Round = bet.Round,
                            PartnerId = bet.OperatorId,
                            BetLogs = betLogs.Where(x => x.Uid == bet.Uid),
                        });
                    }
                }                            
                return (roundResult, result);
            }
            catch(Exception ex)
            {
                return (new CrashGameRoundEntity(), Enumerable.Empty<CrashGameBetReportEntity>());
            }
        }

        private Expression<Func<CrashGameBetEntity, bool>> GetFilterExpression(SpeedUpGameBetReportFilter filter)
        {
            Expression<Func<CrashGameBetEntity, bool>> expression;
            if (string.IsNullOrWhiteSpace(filter.Nickname))
            {
                expression = x => x.Round == filter.RoundId;
            }
            else
            {
                var nickNameRegex = new Regex($"^(?i){filter.Nickname}$");
                expression = x => x.Round == filter.RoundId && nickNameRegex.IsMatch(x.Name);
            }

            if (!string.IsNullOrWhiteSpace(filter.PartnerId))
            {
                expression = expression.And(x => x.OperatorId == filter.PartnerId);
            }      

            if (filter.ExcludeUUS)
            {
                expression = expression.And(x => x.Currency != "UUS");
            }

            return expression;
        }


        private Expression<Func<CrashGameBetLogEntity, bool>> GetFilterBetLogExpression(SpeedUpGameBetReportFilter filter)
        {
            Expression<Func<CrashGameBetLogEntity, bool>> expression;
            if (string.IsNullOrWhiteSpace(filter.Nickname))
            {
                expression = x => x.Round == filter.RoundId;
            }
            else
            {
                var nickNameRegex = new Regex($"^(?i){filter.Nickname}$");
                expression = x => x.Round == filter.RoundId && nickNameRegex.IsMatch(x.Name);
            }

            if (!string.IsNullOrWhiteSpace(filter.PartnerId))
            {
                expression = expression.And(x => x.OperatorId == filter.PartnerId);
            }

            if (filter.ExcludeUUS)
            {
                expression = expression.And(x => x.Currency != "UUS");
            }

            return expression;
        }
    }
}
