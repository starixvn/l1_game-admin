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
        Task<int> GetTotalBots();

        Task<IList<GenerateAccountResult>> GenerateUsers(string[] names);

        Task<GameAccount> GetAccountAsync(string userId, string nickname, bool excludeUUS);

        Task<UpdateMemberSpinResult> UpdateMemberSpins(UpdateSpinRequest updateRequest);

        Task<SpinEntity> GetMemberSpins(int memberId);

        Task<IList<SpinLogEntity>> GetMemberSpinLogs(int memberId);
    }

    public class GameUserService : IGameUserService
    {
        private const int minAmountScale = 2;
        private const int maxAmountScale = 20;
        private const int minCharNameLength = 3;
        private const int maxCharNameLength = 14;

        private readonly ISentryClient sentryClient;

        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;
        private readonly IGenericRepository<SpinEntity> spinRepository;
        private readonly IGenericRepository<SpinLogEntity> spinLogRepository;

        private readonly int minInitAmount;
        private readonly int maxInitAmount;
        

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

            minInitAmount = minAmountScale * botSettings.DefaultMaxBet;
            maxInitAmount = maxAmountScale * botSettings.DefaultMaxBet;
        }

        public async Task<int> GetTotalBots()
        {
            var bots = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot)).ToList();

            if (bots?.Any() == true)
            {
                return bots.Count;
            }

            return 0;
        }

        public async Task<IList<GenerateAccountResult>> GenerateUsers(string[] names)
        {
            var results = new List<GenerateAccountResult>();

            var latestUserInfoUID = await GetLatestUserInfoUID();
            var newAccounts = new List<AccountEntity>();
            var newAccountInfos = new List<AccountInfoEntity>();
            var tuples = new List<Tuple<AccountEntity, AccountInfoEntity>>();
            var defaultHash = BCrypt.Net.BCrypt.HashPassword("bot1234AA", BCrypt.Net.BCrypt.GenerateSalt(12));

            foreach (var name in names)
            {
                latestUserInfoUID += 1;
                var userName = $"{name}{latestUserInfoUID}";
                var password = string.Empty;
                var amount = (new Random()).Next(minInitAmount, maxInitAmount);
                var avatarId = new Random().Next(1, 40);
                var result = new GenerateAccountResult(userName, name, password);

                if (InvalidName(name))
                {
                    result.ErrorDescription = $"Invalid name {name}";
                    results.Add(result);
                    continue;
                }

                try
                {
                    var account = GenerateAccount(userName, defaultHash);
                    var accountInfo = GenerateAccountInfo(latestUserInfoUID, name, amount, avatarId);

                    newAccounts.Add(account);
                    newAccountInfos.Add(accountInfo);
                    tuples.Add(new Tuple<AccountEntity, AccountInfoEntity>(account, accountInfo));
                }
                catch (Exception ex)
                {
                    sentryClient.CaptureException(ex);
                    result.ErrorDescription = ex.Message;
                }

                results.Add(result);
            }

            await accountRepository.InsertManyAsync(newAccounts);

            foreach (var tuple in tuples)
            {
                tuple.Item2.UserId = tuple.Item1.Id.ToString();
            }

            await accountInfoRepository.InsertManyAsync(newAccountInfos);

            return results.ToArray();
        }

        private static bool InvalidName(string name)
        => string.IsNullOrWhiteSpace(name) || name.Length < minCharNameLength || name.Length > maxCharNameLength || !Regex.IsMatch(name, @"^[a-zA-Z0-9]+$");


        private static AccountEntity GenerateAccount(string userName, string hashPassword)
        => new AccountEntity
        {
            Local = new LocalAccount
            {
                Username = userName,
                Password = hashPassword,
                regDate = DateTime.UtcNow
            }
        };


        private static AccountInfoEntity GenerateAccountInfo(int latestUserInfoUID, string name, int amount, int avatarId)
        => new AccountInfoEntity
        {
            Name = name,
            AvatarId = avatarId,
            Red = amount,
            IsBot = true,
            JoinedOn = DateTime.UtcNow,
            UID = latestUserInfoUID
        };

        private Task<int> GetLatestUserInfoUID()
            => Task.Run(() => { return accountInfoRepository.FilterBy(userInfo => userInfo.UID > 0).LastOrDefault()?.UID ?? 0; });

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
                    SpinCount= updateRequest.AvailSpins - memberSpin.AvailSpins
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
