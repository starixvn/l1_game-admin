using System.Threading.Tasks;
using GamesAdmin.Site.Features.GameRoundResult.Models;
using Refit;

namespace GamesAdmin.Site.Features.GameRoundResult
{
    public interface IGameRoundResultApi
    {
        [Post("/GameRoundResult/GetAccessUrl")]
        Task<GameRoundResultAccessUrlResult> GetAccessUrl([Body] GameRoundResultAccessUrlRequest model);
    }
}