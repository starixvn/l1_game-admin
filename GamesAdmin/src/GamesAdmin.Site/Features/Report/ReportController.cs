using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Report
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IMediator mediator;

        public ReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> BigSmallAsync()
        {
            return View(new BigSmallReportViewModel
            {
                Rounds = (await mediator.Send(new GetBigSmallRoundsRequest())).Select(x => new SelectListItem(x.ToString(), x.ToString()))
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_BigSmallRecords", await mediator.Send(new GetBigSmallReportRequest(roundId, nickname, excludeBot)));
        }
    }
}