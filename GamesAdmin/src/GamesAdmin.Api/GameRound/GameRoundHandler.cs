using GamesAdmin.Api.GameRound.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.GameRound
{
    public class GameRoundHandler
        : IRequestHandler<GetAllRequest, IEnumerable<Round>>
        , IRequestHandler<GetLatestRoundRequest, Round>
        , IRequestHandler<GetBetAmountRequest, IEnumerable<BetInfo>>
    {
        private readonly IGameRoundService gameRoundService;

        public GameRoundHandler(IGameRoundService gameRoundService)
        {
            this.gameRoundService = gameRoundService;
        }

        public Task<IEnumerable<Round>> Handle(GetAllRequest request, CancellationToken cancellationToken)
         => gameRoundService.Get();

        public Task<Round> Handle(GetLatestRoundRequest request, CancellationToken cancellationToken)
         => gameRoundService.GetLastestRound();

        public Task<IEnumerable<BetInfo>> Handle(GetBetAmountRequest request, CancellationToken cancellationToken)
         => gameRoundService.GetCurrentBetAmount();
    }
}
