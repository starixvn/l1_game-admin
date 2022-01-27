using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Models.RoundResults;
using Refit;
using System.Threading.Tasks;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface IHoldemPokerApi
    {
        [Post("/api/set-result")]
        Task<BaseCasinoHoldemPokerResultControl> SetResult([Body] SetHoldemPokerResultRequest request);
    }

    public interface ICasinoHoldemPokerApi : IHoldemPokerApi
    {
        [Post("/api/set-result")]
        new Task<BaseCasinoHoldemPokerResultControl> SetResult([Body] SetHoldemPokerResultRequest request);
    }

    public interface ITexasHoldemPokerApi : IHoldemPokerApi
    {
        [Post("/api/set-result")]
        new Task<BaseCasinoHoldemPokerResultControl> SetResult([Body] SetHoldemPokerResultRequest request);
    }
}