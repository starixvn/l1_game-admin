using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.VoidGame.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.VoidGame
{
    public class VoidGameHandler : IRequestHandler<VoidTicketRequest, bool>
    {
        private readonly IVoidGameService voidGameService;

        public VoidGameHandler(IVoidGameService voidGameService)
        {
            this.voidGameService = voidGameService;
        }

        public Task<bool> Handle(VoidTicketRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}