namespace GamesAdmin.Site.Features.PlayerInfo
{
    using MediatR;
    using System.Threading.Tasks;
    using System.Threading;
    using Requests;
    using ViewModels;

    public class PlayerInfoHandler : IRequestHandler<GetPlayerInfoRequest, IndexViewModel>
    {
        private readonly IPlayerInfoService playerInfoService;

        public PlayerInfoHandler(IPlayerInfoService playerInfoService)
        {
            this.playerInfoService = playerInfoService;
        }

        public async Task<IndexViewModel> Handle(GetPlayerInfoRequest request, CancellationToken cancellationToken)
        {
            var playerInfos = await playerInfoService.GetPlayerInfos(request);
            return new IndexViewModel
            {
                Players = playerInfos
            };
        }
    }
}
