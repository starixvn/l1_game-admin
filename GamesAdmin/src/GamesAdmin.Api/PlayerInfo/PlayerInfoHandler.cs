namespace GamesAdmin.Api.PlayerInfo
{
    using MediatR;
    using System.Threading.Tasks;
    using System.Threading;
    using GamesAdmin.Api.PlayerInfo.Requests;
    using System.Collections.Generic;
    using GamesAdmin.Core.Models;

    public class PlayerInfoHandler : IRequestHandler<GetPlayerInfoRequest, IEnumerable<PlayerInfoModel>>
    {
        private readonly IPlayerInfoService playerInfoService;

        public PlayerInfoHandler(IPlayerInfoService playerInfoService)
        {
            this.playerInfoService = playerInfoService;
        }

        public Task<IEnumerable<PlayerInfoModel>> Handle(GetPlayerInfoRequest request, CancellationToken cancellationToken)
            => this.playerInfoService.GetPlayerInfos(request.PlayerId, request.Name);

    }
}
