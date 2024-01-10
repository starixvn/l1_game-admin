using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Core.Models.SlotGame;
using MediatR;
using HiloCard = GamesAdmin.Core.Models.Card;

namespace GamesAdmin.Api.GameServers
{
    public class GameServerHandler
        : IRequestHandler<ReloadRequest, bool>
        , IRequestHandler<JWTRequest, JsonWebToken>
        , IRequestHandler<OnlineUserRequest, OnlineUsers>
        , IRequestHandler<SetSlotBoardRequest, SettlementBoard>
        , IRequestHandler<SetHoldemPokerResultRequest, HoldemPokerResultControl>
        , IRequestHandler<SetHiLoResultRequest, IEnumerable<HiloCard>>
        , IRequestHandler<GetAdminMonitoringSettingsRequest, AdminMonitoringModel>
        , IRequestHandler<SetCascadingSlotResultRequest, IEnumerable<SettlementCascadingSlotResult>>
        , IRequestHandler<VerifyAdminMonitoringTokenRequest, bool>
        , IRequestHandler<ConnectedUserRequest, int>
        , IRequestHandler<SetPontoonResultRequest, bool>
        , IRequestHandler<SetChineseBlackjackResultRequest, bool>
        , IRequestHandler<SetCrashGameResultRequest, bool>
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
            => gameServerService.GetOnlineUsers(request.Game);

        public Task<SettlementBoard> Handle(SetSlotBoardRequest request, CancellationToken cancellationToken)
            => gameServerService.SetSlotBoard(request);

        public Task<HoldemPokerResultControl> Handle(SetHoldemPokerResultRequest request, CancellationToken cancellationToken)
            => gameServerService.SetHoldemPokerResult(request);

        public Task<IEnumerable<HiloCard>> Handle(SetHiLoResultRequest request, CancellationToken cancellationToken)
            => gameServerService.SetHiLoResult(request);

        public Task<bool> Handle(SetPontoonResultRequest request, CancellationToken cancellationToken)
           => gameServerService.SetPontoonResult(request);
        public Task<bool> Handle(SetChineseBlackjackResultRequest request, CancellationToken cancellationToken)
        => gameServerService.SetChineseBlackjackResult(request);

        public Task<AdminMonitoringModel> Handle(GetAdminMonitoringSettingsRequest request, CancellationToken cancellationToken)
            => gameServerService.GetAdminMonitoringSettings(request.Username);

        public Task<IEnumerable<SettlementCascadingSlotResult>> Handle(SetCascadingSlotResultRequest request, CancellationToken cancellationToken)
            => gameServerService.SetCascadingSlotResult(request);

        public Task<bool> Handle(VerifyAdminMonitoringTokenRequest request, CancellationToken cancellationToken)
            => gameServerService.VerifyAdminMonitoringToken(request.Token);

        public Task<int> Handle(ConnectedUserRequest request, CancellationToken cancellationToken)
            => gameServerService.GetConnectedUsers();

        public Task<bool> Handle(SetCrashGameResultRequest request, CancellationToken cancellationToken)
            => gameServerService.SetCrashGameResult(request);
          
    }
}