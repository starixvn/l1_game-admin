using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GameUser.Requests;
using GamesAdmin.Api.GameUser.Results;
using GamesAdmin.Api.GameUsers.Results;
using MediatR;

namespace GamesAdmin.Api.GameUser
{
    public class GameUserHandler
        : IRequestHandler<GetBotCountRequest, int>
        , IRequestHandler<GenerateAccountRandomRequest, IList<GenerateAccountResult>>
        , IRequestHandler<ReviseBotNameRequest, int>
        , IRequestHandler<ReviseBotBalanceRequest, int>
        , IRequestHandler<DisableBotRandomlyRequest, int>
        , IRequestHandler<UpdateSpinRequest, UpdateMemberSpinResult>
    {
        private readonly IGameUserService gameUserService;
        private readonly IBotService botService;

        public GameUserHandler(IGameUserService gameUserService, IBotService botService)
        {
            this.gameUserService = gameUserService;
            this.botService = botService;
        }

        public Task<int> Handle(GetBotCountRequest request, CancellationToken cancellationToken)
        => gameUserService.GetTotalBots();

        public Task<IList<GenerateAccountResult>> Handle(GenerateAccountRandomRequest request, CancellationToken cancellationToken)
        => gameUserService.GenerateUsers(request.Names, request.IsBot);

        public Task<int> Handle(ReviseBotNameRequest request, CancellationToken cancellationToken)
        => gameUserService.ReviseBotNames();

        public Task<int> Handle(ReviseBotBalanceRequest request, CancellationToken cancellationToken)
        => botService.ReviseBalance();

        public Task<int> Handle(DisableBotRandomlyRequest request, CancellationToken cancellationToken)
        => botService.DisableRandomly();

        public Task<UpdateMemberSpinResult> Handle(UpdateSpinRequest request, CancellationToken cancellationToken)
        => gameUserService.UpdateMemberSpins(request);
    }
}
