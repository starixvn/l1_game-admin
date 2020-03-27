using GamesAdmin.Api.UM.Requests;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.UM
{
    public class UMHandler
        : IRequestHandler<ScheduleRequest, bool>
        , IRequestHandler<StartRequest, bool>
        , IRequestHandler<EndRequest, bool>
    {
        private readonly IUMService umService;

        public UMHandler(IUMService umService) 
        {
            this.umService = umService;
        }

        public Task<bool> Handle(ScheduleRequest request, CancellationToken cancellationToken)
        => umService.AddSchedule(request.StartTime, request.EndTime);

        public Task<bool> Handle(StartRequest request, CancellationToken cancellationToken)
        => umService.Start();

        public Task<bool> Handle(EndRequest request, CancellationToken cancellationToken)
        => umService.End();
    }
}
