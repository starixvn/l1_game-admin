using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.CurrencyGroupGames
{
    public interface IGroupCurrencyGameApi : IBaseAuthorizationApi
    {
        [Get("/currencyGroupGame/get")]
        Task<CurrencyGroupGame> Get(string currencyGroupId, int gameId);

        [Get("/currencyGroupGame/getAll")]
        Task<IEnumerable<CurrencyGroupGame>> GetAll();

        [Post("/currencyGroupGame/update")]
        Task<bool> Update(CurrencyGroupGame currencyGroupGame);

        [Get("/currencyGroupGame/getByGroup")]
        Task<IEnumerable<CurrencyGroupGame>> GetByGroup(string currencyGroupId);

        [Get("/currencyGroupGame/config")]
        Task<CurrencyGroupGameConfig> GetConfig(string currencyGroupId, int gameId, string type);

        [Post("/currencyGroupGame/updateConfig")]
        Task<bool> UpdateConfig(string currencyGroupId, int gameId, object configValue, string type);

        [Post("/currencyGroupGame/addRange")]
        Task<bool> AddRange(IEnumerable<CurrencyGroupGame> currencyGroupGames);
    }
}