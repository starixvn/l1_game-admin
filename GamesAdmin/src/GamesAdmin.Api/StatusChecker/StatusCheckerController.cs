using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.StatusChecker;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.ServiceStatus
{
    [Route("api/status_checker")]
    public class StatusCheckerController:  BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public StatusCheckerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("check")]
        public async Task<StatusGameResult> CheckServiceStatus(CheckServiceStatusRequest request)
        => (await mediator.Send(request));
    }
}
