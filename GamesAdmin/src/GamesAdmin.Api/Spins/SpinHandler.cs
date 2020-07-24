using GamesAdmin.Api.Spins.Requests;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Spins
{
    public class SpinHandler
        : IRequestHandler<CreateRequest, bool>
    {
        private readonly ISpinService spinService;

        public SpinHandler(ISpinService spinService) 
        {
            this.spinService = spinService;
        }

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => spinService.AddSpin(request);
    }
}
