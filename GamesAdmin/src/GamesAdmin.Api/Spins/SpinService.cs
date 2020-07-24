using GamesAdmin.Api.Spins.Requests;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using Sentry;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Spins
{
    public interface ISpinService
    {
        Task AddSpin(CreateRequest createRequest);
    }

    public class SpinService : ISpinService
    {
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<AccountEntity> accountRepository;
        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;

        public SpinService(
            ISentryClient sentryClient,
            IGenericRepository<AccountEntity> accountRepository,
            IGenericRepository<AccountInfoEntity> accountInfoRepository)
        {
            this.sentryClient = sentryClient;
            this.accountRepository = accountRepository;
            this.accountInfoRepository = accountInfoRepository;
        }

        public async Task AddSpin(CreateRequest createRequest)
        {
            var user = await accountRepository.FindOneAsync(account => account.Local.MemberId == createRequest.ObCustId);
            var userInfo = await accountInfoRepository.FindOneAsync(accountInfo => accountInfo.UserId == user.Id.ToString());

            var totalSpins = userInfo.Spins += createRequest.Spins;

            Builders<AccountInfoEntity>.Update.Set(nameof(AccountInfoEntity.Spins), totalSpins);

            await accountInfoRepository.UpdateAsync(
                accountInfo => accountInfo.UserId == user.Id.ToString(),
                Builders<AccountInfoEntity>.Update.Set(nameof(AccountInfoEntity.Spins), totalSpins));
        }
    }
}
