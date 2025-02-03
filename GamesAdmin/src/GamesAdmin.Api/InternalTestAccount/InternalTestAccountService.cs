namespace GamesAdmin.Api
{
    using GamesAdmin.Database;
    using GamesAdmin.Database.Entities;
    using MongoDB.Driver;
    using Sentry;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IInternalTestAccountService
    {
        Task<string[]> Get();

        Task<bool> UpdateInternalTest(string[] addAccounts, string[] deletedAccounts);
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

        public async Task<bool> UpdateInternalTest(string[] addAccounts, string[] deletedAccounts)
        {
            try
            {
                if (addAccounts.Length == 0 && deletedAccounts.Length == 0)
                {
                    return true;
                }

                if (addAccounts.Length > 0)
                {
                    var updateTrue = Builders<UserEntity>.Update.Set(x => x.IsInternal, true);
                    await userRepository.UpdateManyAsync(
                        x => addAccounts.Contains(x.Local.Username),
                        updateTrue
                    );
                }

                if (deletedAccounts.Length > 0)
                {
                    var updateFalse = Builders<UserEntity>.Update.Set(x => x.IsInternal, false);
                    await userRepository.UpdateManyAsync(
                        x => deletedAccounts.Contains(x.Local.Username),
                        updateFalse
                    );
                }

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }
    }
}
