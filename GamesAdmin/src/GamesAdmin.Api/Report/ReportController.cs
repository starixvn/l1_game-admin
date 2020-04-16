using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Report.Request;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Report
{
    [Route("api/report")]
    public class ReportController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public ReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("big_small/round/{roundId}/users/{nickname?}")]
        public async Task<BigSmallReport> BigSmall(long roundId, string nickname, bool excludeBot)
           => (await mediator.Send(new BigSmallReportRequest(roundId, nickname, excludeBot)));
    }
}