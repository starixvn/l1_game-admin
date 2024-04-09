using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using Refit;

namespace GamesAdmin.Api.GameManagementApi
{
    [Headers("Authorization: Bearer")]
    public interface IGameManagementApi
    {
        [Get("/game-configs/byid")]
        Task<GameConfigResponse> GetGameConfigById([Query] int gameId);
    }

    public class GameConfigResponse
    {
        public GameConfigEntity Data { get; set; }
    }
}