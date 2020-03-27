using GamesAdmin.Api.UM.Requests;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.UM
{
    public class UMHandler
        : IRequestHandler<ScheduleRequest, bool>
    {
        public Task<bool> Handle(ScheduleRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
