using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.Spins;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.GameUsers
{
    public interface IGameUserApi : IBaseAuthorizationApi
    {
        [Get("/game_users/bot/count")]
        Task<int> BotCount();

        [Get("/game_users/spins/{memberId}/get")]
        Task<Spin> GetMemberSpins(int memberId);

        [Get("/game_users/spins/{memberId}/get_logs")]
        Task<List<SpinLog>> GetMemberSpinLogs(int memberId);

        [Post("/game_users/spins/update")]
        Task UpdateMemberSpin();
    }

    public interface IGameUserService
    {
        Task<int> GetBotCount();

        Task<Spin> GetMemberSpins(int memberId);

        Task<List<SpinLog>> GetMemberSpinLogs(int memberId);
    }

    public class GameUserService : IGameUserService
    {
        private readonly IGameUserApi gameUserApi;

        public GameUserService(IGameUserApi accountApi)
        {
            this.gameUserApi = accountApi;
        }

        public Task<int> GetBotCount()
        => gameUserApi.BotCount();

        public Task<List<SpinLog>> GetMemberSpinLogs(int memberId)
        => gameUserApi.GetMemberSpinLogs(memberId);

        public Task<Spin> GetMemberSpins(int memberId)
        => gameUserApi.GetMemberSpins(memberId);
    }
}