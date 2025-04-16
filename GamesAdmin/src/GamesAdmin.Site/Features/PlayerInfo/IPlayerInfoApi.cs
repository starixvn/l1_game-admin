namespace GamesAdmin.Site.Features.PlayerInfo
{
    using GamesAdmin.Core.Models;
    using GamesAdmin.Site.Features._Shared;
    using Refit;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Requests;

    public interface IPlayerInfoApi : IBaseAuthorizationApi
    {
        [Post("/playerInfo/get")]
        Task<IEnumerable<PlayerInfoModel>> Get(GetPlayerInfoRequest request);
    }
}
