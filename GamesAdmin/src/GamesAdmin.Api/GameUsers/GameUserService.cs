using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.GameUser.Requests;
using GamesAdmin.Api.GameUser.Results;
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

        Task<UpdateMemberSpinResult> UpdateMemberSpins(UpdateSpinRequest updateRequest);

        Task<SpinEntity> GetMemberSpins(int memberId);

        Task<IList<SpinLogEntity>> GetMemberSpinLogs(int memberId);
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
            IBotSettings botSettings,
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
                .FindOneAsync(x => x.UserId == account.Id.ToString()
                    && IsExcludeUUS(excludeUUS, x.Currency)
                    && (string.IsNullOrWhiteSpace(nickname) || nickNameRegex.IsMatch(x.Name)));

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

        private bool IsExcludeUUS(bool excludeUUS, string currency) => (!excludeUUS || (currency != "UUS"));

        public async Task<UpdateMemberSpinResult> UpdateMemberSpins(UpdateSpinRequest updateRequest)
        {
            var result = new UpdateMemberSpinResult { MemberId = updateRequest.MemberId };

            try
            {
                var memberSpin = await GetMemberSpins(updateRequest.MemberId);
                var update = Builders<SpinEntity>.Update.Set(nameof(SpinEntity.AvailSpins), updateRequest.AvailSpins);

                await spinRepository.UpdateAsync(spin => spin.MemberId == updateRequest.MemberId, update);

                var log = new SpinLogEntity
                {
                    Key = Guid.NewGuid().ToString(),
                    MemberId = updateRequest.MemberId,
                    Source = $"admin_{updateRequest.AdminUser}",
                    ExpiryDate = updateRequest.ExpiryDate,
                    SpinCount = updateRequest.AvailSpins - memberSpin.AvailSpins
                };

                await spinLogRepository.InsertOneAsync(log);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return result;
        }

        public Task<SpinEntity> GetMemberSpins(int memberId)
        => spinRepository.FindOneAsync(spin => spin.MemberId == memberId);

        public async Task<IList<SpinLogEntity>> GetMemberSpinLogs(int memberId)
        => (await spinLogRepository
            .FilterByAsync(spin => spin.MemberId == memberId))
            .OrderByDescending(spin => spin.ExpiryDate)
            .ToList();
    }
}
