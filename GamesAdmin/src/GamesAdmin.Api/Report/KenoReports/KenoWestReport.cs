using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Keno;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public class KenoWestReport : IKenoReport
    {
        private readonly IGenericRepository<KenoWestOnesEntity> kenoWestOnesRepository;
        private readonly IGenericRepository<KenoWestRoundEntity> kenoWestRoundRepository;
        private readonly IGenericRepository<KenoWestBetEntity> kenoWestBetRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;
        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;

        public KenoWestReport(
            IGenericRepository<KenoWestOnesEntity> kenoWestOnesRepository,
            IGenericRepository<KenoWestRoundEntity> kenoWestRoundRepository,
            IGenericRepository<KenoWestBetEntity> kenoWestBetRepository,
            IGenericRepository<AccountEntity> accountRepository,
            IGenericRepository<AccountInfoEntity> accountInfoRepository)
        {
            this.kenoWestOnesRepository = kenoWestOnesRepository;
            this.kenoWestRoundRepository = kenoWestRoundRepository;
            this.kenoWestBetRepository = kenoWestBetRepository;

            this.accountRepository = accountRepository;
            this.accountInfoRepository = accountInfoRepository;
        }

        public async Task<IEnumerable<KenoBetHistory>> GetBetHistoryAsync(long roundId, string nickname)
        {
            var bets = await kenoWestBetRepository.FilterByAsync(bet => bet.Round == roundId && bet.Name == nickname);

            var betLogs = bets
                .Select(bet => new KenoBetHistory(
                    bet.Round,
                    bet.Name,
                    Enumeration.FromValue<KenoBetChoice>(bet.Choice),
                    bet.Amount,
                    bet.MemberId,
                    bet.SiteId,
                    new DateTimeOffset(bet.Time)))
                .ToList();

            return betLogs;
        }

        public async Task<IEnumerable<KenoBetRecord>> GetReportAsync(long roundId, string nickname, bool excludeUUS)
        {
            var bets = await kenoWestOnesRepository.FilterByAsync(x => x.Round == roundId);

            var betRecords = bets
                .Select(bet => new KenoBetRecord(bet.ConvertAmountToDictionary(), bet.Odds)
                {
                    UserId = bet.UserId.ToString(),
                    Nickname = bet.Name,
                    BetWin = bet.Betwin,
                    Time = bet.Time
                })
                .ToList();

            foreach (var bet in betRecords)
            {
                bet.Account = await GetAccountAsync(bet.UserId, nickname, excludeUUS);
            }

            return betRecords.Where(bet => bet.Account != null);
        }

        private async Task<GameAccount> GetAccountAsync(string userId, string nickname, bool excludeUUS)
        {
            var nickNameRegex = new Regex($"(?i){nickname}");
            var accounts = await accountRepository
                .Aggregate()
                .Match(x => x.Id == ObjectId.Parse(userId)
                    && (string.IsNullOrWhiteSpace(nickname) || nickNameRegex.IsMatch(x.Local.Username)))
                .AppendStage<AccountEntity>("{$addFields: {userId: {$toObjectId: '$id}}}")
                .Lookup<AccountEntity, AccountInfoEntity, JoinedAccountInfoEntity>(
                      accountInfoRepository.Collection(),
                      x => x.Id,
                      y => y.Id,
                      y => y.InnerUserInfo
                  )
                .ToListAsync();

            return accounts
                .Select(x => new GameAccount(
                    x.Local.Username,
                    x.Local.MemberId,
                    x.Local.MemberName,
                    x.Local.SiteId,
                    x.Local.Language,
                    x.InnerUserInfo.FirstOrDefault()?.IsBot,
                    x.InnerUserInfo.FirstOrDefault()?.Currency,
                    x.InnerUserInfo.FirstOrDefault()?.Name,
                    x.InnerUserInfo.FirstOrDefault()?.UID
                ))
                .Where(x => (excludeUUS && !x.IsUUS))
                .FirstOrDefault();
        }

        public async Task<KenoRoundResult> GetRoundResultAsync(long roundId)
        {
            var round = await kenoWestRoundRepository.FindOneAsync(round => round.Number == roundId);

            return round == null || round.Result == null || round.SettlementResult == null
                ? default
                : new KenoRoundResult(
                    round.Result.ToArray(),
                    new KenoSettlementResult(round.SettlementResult),
                    round.Time);
        }
    }
}
