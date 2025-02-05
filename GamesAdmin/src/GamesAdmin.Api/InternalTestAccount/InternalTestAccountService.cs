namespace GamesAdmin.Api
{
    using GamesAdmin.Core.Models;
    using GamesAdmin.Database;
    using GamesAdmin.Database.Entities;
    using MongoDB.Driver;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IInternalTestAccountService
    {
        Task<string[]> Get();

        Task<UpdateInternalUUSTestResult> UpdateInternalTest(string[] addAccounts, string[] deletedAccounts);
    }

    public class InternalTestAccountService : IInternalTestAccountService
    {
        private readonly IGenericRepository<UserEntity> userRepository;

        private readonly ISentryClient sentryClient;

        public InternalTestAccountService(IGenericRepository<UserEntity> userRepository, ISentryClient sentryClient)
        {
            this.userRepository = userRepository;
            this.sentryClient = sentryClient;
        }

        public async Task<string[]> Get()
        {
            try
            {
                var userEntities = await userRepository.FilterByAsync(x => (bool)x.IsInternal && x.IsInternal == true);
                var usernames = userEntities.Select(x => x.Local.Username).ToArray();

                return usernames;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Array.Empty<string>();
            }
        }

        public async Task<UpdateInternalUUSTestResult> UpdateInternalTest(string[] addAccounts, string[] deletedAccounts)
        {
            try
            {
                if (IsInputEmpty(addAccounts, deletedAccounts))
                {
                    return SuccessResult();
                }

                var invalidAccounts = new List<string>();

                if (addAccounts?.Length > 0)
                {
                    invalidAccounts = await ProcessAccounts(addAccounts, true);
                    if (invalidAccounts.Any())
                    {
                        return FailureResult(invalidAccounts);
                    }
                }

                if (deletedAccounts?.Length > 0)
                {
                    invalidAccounts = await ProcessAccounts(deletedAccounts, false);
                    if (invalidAccounts.Any())
                    {
                        return FailureResult(invalidAccounts);
                    }
                }

                return SuccessResult();
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return FailureResult(new List<string>());
            }
        }

        private static bool IsInputEmpty(string[] addAccounts, string[] deletedAccounts)
        {
            return (addAccounts == null || addAccounts.Length == 0) &&
                   (deletedAccounts == null || deletedAccounts.Length == 0);
        }

        private static UpdateInternalUUSTestResult SuccessResult()
        {
            return new UpdateInternalUUSTestResult(true, new List<string>());
        }

        private static UpdateInternalUUSTestResult FailureResult(List<string> invalidAccounts)
        {
            return new UpdateInternalUUSTestResult(false, invalidAccounts);
        }

        private async Task<List<string>> ProcessAccounts(string[] accounts, bool isAdding)
        {
            var accountsIgnoreCase = NormalizeToHashSet(accounts);

            var existingUsers = await FindExistingUsers(accountsIgnoreCase);
            var existingUsernames = existingUsers.Select(u => u.Local.Username.ToLowerInvariant());

            var invalidAccounts = accountsIgnoreCase.Where(acc => !existingUsernames.Contains(acc.ToLowerInvariant())).ToList();

            if (invalidAccounts.Any())
            {
                return invalidAccounts;
            }

            await UpdateAccounts(accountsIgnoreCase, isAdding);

            return new List<string>();
        }

        private static HashSet<string> NormalizeToHashSet(IEnumerable<string> accounts)
        {
            return new HashSet<string>(accounts, StringComparer.OrdinalIgnoreCase);
        }

        private async Task<List<UserEntity>> FindExistingUsers(HashSet<string> accounts)
        {
            return (await userRepository.FilterByAsync(x => accounts.Contains(x.Local.Username))).ToList();
        }

        private async Task UpdateAccounts(HashSet<string> accounts, bool isAdding)
        {
            var update = Builders<UserEntity>.Update.Set(x => x.IsInternal, isAdding);
            await userRepository.UpdateManyAsync(x => accounts.Contains(x.Local.Username), update);
        }
    }
}
