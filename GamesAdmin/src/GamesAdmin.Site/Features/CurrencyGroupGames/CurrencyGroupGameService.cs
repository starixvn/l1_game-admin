using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;

namespace GamesAdmin.Site.Features.CurrencyGroupGames
{
    public interface ICurrencyGroupGameService
    {
        Task<IEnumerable<CurrencyGroupGame>> GetAll();

        Task<ResultBase> Update(CurrencyGroupGame currencyGroupGame);

        Task<CurrencyGroupGame> Get(string groupId, int gameId);
    }

    public class CurrencyGroupGameService : ICurrencyGroupGameService
    {
        private readonly IGroupCurrencyGameApi groupCurrencyGameApi;

        public CurrencyGroupGameService(IGroupCurrencyGameApi groupCurrencyGameApi)
        {
            this.groupCurrencyGameApi = groupCurrencyGameApi;
        }

        public Task<CurrencyGroupGame> Get(string groupId, int gameId)
            => groupCurrencyGameApi.Get(groupId, gameId);

        public Task<IEnumerable<CurrencyGroupGame>> GetAll()
            => groupCurrencyGameApi.GetAll();

        public async Task<ResultBase> Update(CurrencyGroupGame currencyGroupGame)
        {
            var updated = await groupCurrencyGameApi.Update(currencyGroupGame);

            return new ResultBase(!updated ? "Cannot update settings" : string.Empty);
        }

        public Task<IEnumerable<CurrencyGroupGame>> GetByGroup(string currencyGroupId)
            => groupCurrencyGameApi.GetByGroup(currencyGroupId);
    }
}