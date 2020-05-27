using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.VietlottKeno.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.VietlottKeno
{
    public class VietlottKenoHandler :
        IRequestHandler<GetRoundsRequest, IEnumerable<VietlottKenoRound>>,
        IRequestHandler<CreateRoundRequest, Unit>
    {
        private readonly IVietlottKenoService vietlottKenoService;

        public VietlottKenoHandler(IVietlottKenoService vietlottKenoService)
        {
            this.vietlottKenoService = vietlottKenoService;
        }

        public async Task<IEnumerable<VietlottKenoRound>> Handle(GetRoundsRequest request, CancellationToken cancellationToken)
        => await vietlottKenoService.GetRoundsAsync(request.Date);

        public async Task<Unit> Handle(CreateRoundRequest request, CancellationToken cancellationToken)
        {
            await vietlottKenoService.CreateAsync(request.KenoRound);
            return Unit.Value;
        }
    }
}