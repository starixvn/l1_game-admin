using System.Threading.Tasks;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult BigSmall()
        {
            return View(new BigSmallReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_BigSmallRecords", await mediator.Send(new GetBigSmallReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public IActionResult BigSmallTurbo()
        {
            return View(new BigSmallTurboReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallTurboReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_BigSmallTurboRecords", await mediator.Send(new GetBigSmallTurboReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public IActionResult OddEven()
        {
            return View(new OddEvenReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetOddEvenReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_OddEvenRecords", await mediator.Send(new GetOddEvenReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public IActionResult OddEvenTurbo()
        {
            return View(new OddEvenTurboReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetOddEvenTurboReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_OddEvenTurboRecords", await mediator.Send(new GetOddEvenTurboReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public IActionResult ShakeThePlate()
        {
            return View(new ShakeThePlateReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetShakeThePlateReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_ShakeThePlateRecords", await mediator.Send(new GetShakeThePlateReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallBetHistory(long roundId, string nickname)
        {
            return PartialView("_BigSmallBetHistory", await mediator.Send(new GetBigSmallBetHistoryRequest(roundId, nickname)));
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallTurboBetHistory(long roundId, string nickname)
        {
            return PartialView("_BigSmallBetHistory", await mediator.Send(new GetBigSmallTurboBetHistoryRequest(roundId, nickname)));
        }

        [HttpGet]
        public async Task<IActionResult> GetOddEvenBetHistory(long roundId, string nickname)
        {
            return PartialView("_OddEvenBetHistory", await mediator.Send(new GetOddEvenBetHistoryRequest(roundId, nickname)));
        }

        [HttpGet]
        public async Task<IActionResult> GetOddEvenTurboBetHistory(long roundId, string nickname)
        {
            return PartialView("_OddEvenBetHistory", await mediator.Send(new GetOddEvenTurboBetHistoryRequest(roundId, nickname)));
        }
    }
}