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

        [Post("/currencyGroupGame/update")]
        Task<bool> Update(CurrencyGroupGame currencyGroupGame);
    }
}