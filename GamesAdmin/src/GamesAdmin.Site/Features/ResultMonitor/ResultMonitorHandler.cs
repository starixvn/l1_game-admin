using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.ResultMonitor.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.ResultMonitor
{
    public class ResultMonitorHandler : IRequestHandler<SetSlotResultRequest, bool>
    {
        private readonly IResultMonitorService service;

        public ResultMonitorHandler(IResultMonitorService service)
        {
            this.service = service;
        }

        public Task<bool> Handle(SetSlotResultRequest request, CancellationToken cancellationToken)
        => service.SetSlotResult(request);
    }
}