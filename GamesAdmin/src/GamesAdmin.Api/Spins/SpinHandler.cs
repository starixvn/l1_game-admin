using GamesAdmin.Api.Spins.Requests;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Spins
{
    public class SpinHandler
        : IRequestHandler<CreateRequest>
    {
        public Task<Unit> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
