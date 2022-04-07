using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Core.Models.SlotGame;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.GameServers
{
    public class GameServerHandler
        : IRequestHandler<ReloadRequest, bool>
        , IRequestHandler<JWTRequest, JsonWebToken>
        , IRequestHandler<OnlineUserRequest, OnlineUsers>
        , IRequestHandler<NotifyRoundsRequest, bool>
        , IRequestHandler<SetSlotBoardRequest, SettlementBoard>
        , IRequestHandler<SetHoldemPokerResultRequest, HoldemPokerResultControl>
        , IRequestHandler<SetHiLoResultRequest, IEnumerable<PokerCard>>
        , IRequestHandler<GetAdminMonitoringSettingsRequest, AdminMonitoringModel>
        , IRequestHandler<VerifyAdminMonitoringTokenRequest, bool>
    {
        private readonly IGameServerService gameServerService;

        public GameServerHandler(IGameServerService gameServerService)
        {
            this.gameServerService = gameServerService;
        }

        public async Task<bool> Handle(ReloadRequest request, CancellationToken cancellationToken)
        {
            await gameServerService.Reload();

            return true;
        }

        public Task<JsonWebToken> Handle(JWTRequest request, CancellationToken cancellationToken)
            => gameServerService.GetJWT(request);

        public Task<OnlineUsers> Handle(OnlineUserRequest request, CancellationToken cancellationToken)
            => string.IsNullOrWhiteSpace(request.Game)
                ? gameServerService.GetOnlineUsers()
                : gameServerService.GetOnlineUsers(Enumeration.FromValue<Game>(request.Game));

        public Task<bool> Handle(NotifyRoundsRequest request, CancellationToken cancellationToken)
            => gameServerService.NotifyRoundResult(request.Rounds);

        public Task<SettlementBoard> Handle(SetSlotBoardRequest request, CancellationToken cancellationToken)
            => gameServerService.SetSlotBoard(request);

        public Task<HoldemPokerResultControl> Handle(SetHoldemPokerResultRequest request, CancellationToken cancellationToken)
            => gameServerService.SetHoldemPokerResult(request);

        public Task<IEnumerable<PokerCard>> Handle(SetHiLoResultRequest request, CancellationToken cancellationToken)
         => gameServerService.SetHiLoResult(request);

        public Task<AdminMonitoringModel> Handle(GetAdminMonitoringSettingsRequest request, CancellationToken cancellationToken)
            => gameServerService.GetAdminMonitoringSettings(request.Username);

        public Task<bool> Handle(VerifyAdminMonitoringTokenRequest request, CancellationToken cancellationToken)
            => gameServerService.VerifyAdminMonitoringToken(request.Token);
    }
}
