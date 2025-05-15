namespace GamesAdmin.Site.Features.PlayerInfo
{
    using GamesAdmin.Core.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Requests;
    public interface IPlayerInfoService
    {
        Task<IEnumerable<PlayerInfoModel>> GetPlayerInfos(GetPlayerInfoRequest request);
    }
    public class PlayerInfoService : IPlayerInfoService
    {
        private readonly IPlayerInfoApi playerInfoApi;

        public PlayerInfoService(IPlayerInfoApi playerInfoApi)
        {
            this.playerInfoApi = playerInfoApi;
        }

        public Task<IEnumerable<PlayerInfoModel>> GetPlayerInfos(GetPlayerInfoRequest request)
           => playerInfoApi.Get(request);
    }
}
