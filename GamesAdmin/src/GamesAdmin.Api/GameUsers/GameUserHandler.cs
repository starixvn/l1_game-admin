using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GameUser.Requests;
using GamesAdmin.Api.GameUser.Results;
using GamesAdmin.Database.Entities.Spins;
using MediatR;

namespace GamesAdmin.Api.GameUser
{
    public class GameUserHandler
        : IRequestHandler<GetBotCountRequest, int>
        , IRequestHandler<GenerateAccountRandomRequest, IList<GenerateAccountResult>>
        , IRequestHandler<ReviseBotBalanceRequest, int>
        , IRequestHandler<DisableBotRandomlyRequest, int>
        , IRequestHandler<UpdateSpinRequest, UpdateMemberSpinResult>
        , IRequestHandler<GetSpinRequest, SpinEntity>
        , IRequestHandler<GetSpinLogRequest, IList<SpinLogEntity>>
        , IRequestHandler<DeactivateBotRequest, int>
    {
        private readonly IGameUserService gameUserService;
        private readonly IBotService botService;

        public GameUserHandler(IGameUserService gameUserService, IBotService botService)
        {
            this.gameUserService = gameUserService;
            this.botService = botService;
        }

        public Task<int> Handle(GetBotCountRequest request, CancellationToken cancellationToken)
        => botService.GetTotal();

        public Task<IList<GenerateAccountResult>> Handle(GenerateAccountRandomRequest request, CancellationToken cancellationToken)
        => botService.Generate(request.Names);

        public Task<int> Handle(ReviseBotBalanceRequest request, CancellationToken cancellationToken)
        => botService.ReviseBalance();

        public Task<int> Handle(DisableBotRandomlyRequest request, CancellationToken cancellationToken)
        => botService.DisableRandomly();

        public Task<UpdateMemberSpinResult> Handle(UpdateSpinRequest request, CancellationToken cancellationToken)
        => gameUserService.UpdateMemberSpins(request);

        public Task<SpinEntity> Handle(GetSpinRequest request, CancellationToken cancellationToken)
        => gameUserService.GetMemberSpins(request.MemberId);

        public Task<IList<SpinLogEntity>> Handle(GetSpinLogRequest request, CancellationToken cancellationToken)
        => gameUserService.GetMemberSpinLogs(request.MemberId);

        public Task<int> Handle(DeactivateBotRequest request, CancellationToken cancellationToken)
        => botService.Deactivate();
    }
}
