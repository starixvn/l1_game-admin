using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.ProVietKeno.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.ProVietKeno
{
    public class ProVietKenoHandler :
        IRequestHandler<GetRoundsRequest, IEnumerable<ProVietKenoRound>>,
        IRequestHandler<CreateRoundRequest, Unit>
    {
        private readonly IProVietKenoService provietKenoService;

        public ProVietKenoHandler(IProVietKenoService provietKenoService)
        {
            this.provietKenoService = provietKenoService;
        }

        public async Task<IEnumerable<ProVietKenoRound>> Handle(GetRoundsRequest request, CancellationToken cancellationToken)
        => await provietKenoService.GetRoundsAsync(request.Date);

        public async Task<Unit> Handle(CreateRoundRequest request, CancellationToken cancellationToken)
        {
            await provietKenoService.CreateAsync(request.KenoRound);
            return Unit.Value;
        }
    }
}