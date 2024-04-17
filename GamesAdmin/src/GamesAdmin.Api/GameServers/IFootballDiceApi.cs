using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Models.FootballDice;
using Refit;
using System.Threading.Tasks;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface IFootballDiceApi
    {
        [Post("/api/set-result")]
        Task<FootballDiceControlResult> SetResult([Body] SetFootballDiceResultRequest request);
    }
}
