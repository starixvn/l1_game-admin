using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.GameRoundResult.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.GameRoundResult
{
    public class GameRoundResultHandler : IRequestHandler<GetAccessUrlRequest, string>
    {
        private readonly IGameRoundResultService gameRoundResultService;

        public GameRoundResultHandler(IGameRoundResultService gameRoundResultService)
        {
            this.gameRoundResultService = gameRoundResultService;
        }

        public async Task<string> Handle(GetAccessUrlRequest request, CancellationToken cancellationToken)
        {
            return await gameRoundResultService.GetAccessUrlAsync();
        }
    }
}