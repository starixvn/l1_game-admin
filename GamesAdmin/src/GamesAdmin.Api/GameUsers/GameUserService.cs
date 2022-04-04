using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Spins;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.GameUser
{
    public interface IGameUserService
    {
        Task<GameAccount> GetAccountAsync(string userId, string nickname, bool excludeUUS);

        Task<SpinEntity> GetMemberSpins(int memberId);

        Task<IList<SpinLogEntity>> GetMemberSpinLogs(int memberId);

        Task<bool> IsAbleToSetSlotResult(string nickname, bool excludeUUS);

        Task<string> GetCharacterNameAsync(int memberId);
    }

    public class GameUserService : IGameUserService
    {
        private readonly ISentryClient sentryClient;

        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;
        private readonly IGenericRepository<SpinEntity> spinRepository;
        private readonly IGenericRepository<SpinLogEntity> spinLogRepository;

        public GameUserService(
            ISentryClient sentryClient,
            IGenericRepository<AccountInfoEntity> accountInfoRepository,
            IGenericRepository<AccountEntity> accountRepository,
            IGenericRepository<SpinEntity> spinRepository,
            IGenericRepository<SpinLogEntity> spinLogRepository)
        {
            this.sentryClient = sentryClient;
            this.accountInfoRepository = accountInfoRepository;
            this.accountRepository = accountRepository;
            this.spinRepository = spinRepository;
            this.spinLogRepository = spinLogRepository;
        }

        public async Task<GameAccount> GetAccountAsync(string userId, string nickname, bool excludeUUS)
        {
            var nickNameRegex = new Regex($"(?i){nickname}");

            var account = await accountRepository.FindByIdAsync(userId);
            var accountInfo = await accountInfoRepository
#pragma warning disable S1067 // Expressions should not be too complex
                    .FindOneAsync(x => x.UserId == account.Id.ToString()
                        && (!excludeUUS || x.Currency != "UUS")
                        && (string.IsNullOrWhiteSpace(nickname) || nickNameRegex.IsMatch(x.Name)));
#pragma warning restore S1067 // Expressions should not be too complex
            return accountInfo == null || account == null
            ? default
            : new GameAccount(
                account.Local.Username,
                account.Local.MemberId,
                account.Local.MemberName,
                account.Local.SiteId,
                account.Local.Language,
                accountInfo.IsBot,
                accountInfo.Currency,
                accountInfo.Name,
                accountInfo.UID
            );
        }

        public Task<SpinEntity> GetMemberSpins(int memberId)
        => spinRepository.FindOneAsync(spin => spin.MemberId == memberId);

        public async Task<IList<SpinLogEntity>> GetMemberSpinLogs(int memberId)
        => (await spinLogRepository
            .FilterByAsync(spin => spin.MemberId == memberId))
            .OrderByDescending(spin => spin.ExpiryDate)
            .ToList();

        public async Task<bool> IsAbleToSetSlotResult(string nickname, bool excludeUUS)
        {
            var accountInfo = await accountInfoRepository.FindOneAsync(x => x.Name == nickname);

            sentryClient.CaptureMessage($"Unable to set board for {nickname}, currency {accountInfo?.Currency} with excludeUUS {excludeUUS}");

            return (!excludeUUS && accountInfo?.Currency == "UUS") || (accountInfo?.IsBot == true && accountInfo?.Disabled == false);
        }

        public async Task<string> GetCharacterNameAsync(int memberId)
        {
            var account = await accountRepository.Collection()
                .Find(user => user.Local.MemberId == memberId)
                .SortByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (account != null)
            {
                var accountInfo = await accountInfoRepository.FindOneAsync(x => x.UserId == account.Id.ToString());

                return accountInfo.Name;
            }

            return string.Empty;
        }
    }
}