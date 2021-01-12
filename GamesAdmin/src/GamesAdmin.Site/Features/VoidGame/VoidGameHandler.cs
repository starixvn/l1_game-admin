using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Site.Features.VoidGame.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.VoidGame
{
    public class VoidGameHandler : IRequestHandler<VoidTicketRequest, VoidGameResult>
    {
        private readonly IVoidGameService voidGameService;

        public VoidGameHandler(IVoidGameService voidGameService)
        {
            this.voidGameService = voidGameService;
        }

        public Task<VoidGameResult> Handle(VoidTicketRequest request, CancellationToken cancellationToken)
        => voidGameService.VoidTicket(request);
    }
}