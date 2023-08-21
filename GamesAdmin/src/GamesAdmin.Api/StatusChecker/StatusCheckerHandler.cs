using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.StatusChecker;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.ServiceStatus
{
    public class StatusCheckerHandler: IRequestHandler<CheckServiceStatusRequest, StatusGameResult>
    {
        private readonly IStatusCheckerService statusCheckerService;

        public StatusCheckerHandler(IStatusCheckerService statusCheckerService)
        {
            this.statusCheckerService = statusCheckerService;
        }

        public Task<StatusGameResult> Handle(CheckServiceStatusRequest request, CancellationToken cancellationToken)
        => statusCheckerService.CheckServiceStatus(request);
    }
}
