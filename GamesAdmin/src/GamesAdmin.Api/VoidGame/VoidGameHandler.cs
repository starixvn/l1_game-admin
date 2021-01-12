using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Core.OWResults;
using MediatR;

namespace GamesAdmin.Api.VoidGame
{
    public class VoidGameHandler
        : IRequestHandler<VoidTicketRequest, VoidGameResult>
    {
        private readonly IVoidGameService voidGameService;

        public VoidGameHandler(IVoidGameService voidGameService)
        {
            this.voidGameService = voidGameService;
        }

        public Task<VoidGameResult> Handle(VoidTicketRequest request, CancellationToken cancellationToken)
        => voidGameService.VoidGameTicket(request);
    }
}