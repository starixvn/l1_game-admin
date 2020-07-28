using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.Report
{
    //[Authorize]
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
            return View(new ReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_BigSmallRecords", await mediator.Send(new GetBigSmallReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public IActionResult BigSmallTurbo()
        {
            return View(new ReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallTurboReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_BigSmallTurboRecords", await mediator.Send(new GetBigSmallTurboReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public IActionResult OddEven()
        {
            return View(new ReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetOddEvenReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_OddEvenRecords", await mediator.Send(new GetOddEvenReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public IActionResult OddEvenTurbo()
        {
            return View(new ReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetOddEvenTurboReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_OddEvenTurboRecords", await mediator.Send(new GetOddEvenTurboReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public IActionResult SeDie()
        {
            return View(new ReportViewModel());
        }

        [HttpGet]
        public IActionResult MiniSeDie()
        {
            return View(new ReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetSeDieReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_SeDieRecords", await mediator.Send(new GetSeDieReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public async Task<IActionResult> GetMiniSeDieReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_SeDieRecords", await mediator.Send(new GetMiniSeDieReportRequest(roundId, nickname, excludeBot)));
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

        [HttpGet]
        public IActionResult KenoProViet()
        {
            return View("KenoReport", new KenoViewModel());
        }

        [HttpGet]
        public IActionResult KenoMax()
        {
            return View("KenoReport", new KenoViewModel());
        }

        [HttpGet]
        public IActionResult KenoMax2()
        {
            return View("KenoReport", new KenoViewModel());
        }

        [HttpGet]
        public IActionResult KenoMini()
        {
            return View("KenoReport", new KenoViewModel());
        }

        [HttpGet]
        public IActionResult KenoMini2()
        {
            return View("KenoReport", new KenoViewModel());
        }

        [HttpGet]
        public IActionResult KenoEast()
        {
            return View("KenoReport", new KenoViewModel());
        }

        [HttpGet]
        public IActionResult KenoWest()
        {
            return View("KenoReport", new KenoViewModel());
        }

        [HttpGet]
        public IActionResult KenoSouth()
        {
            return View("KenoReport", new KenoViewModel());
        }

        [HttpGet]
        public IActionResult KenoNorth()
        {
            return View("KenoReport", new KenoViewModel());
        }

        [HttpGet]
        public IActionResult KenoProMax()
        {
            return View("KenoReport", new KenoViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetKenoReport(string game, long roundId, string nickname = null, bool excludeUUS = true)
        {
            return PartialView("_KenoBetRecords", await mediator.Send(new GetKenoReportRequest(
                Enumeration.FromValue<GameType>(game),
                roundId,
                nickname,
                excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> KenoBetHistory(string game, long roundId, string nickname)
        {
            return View(await mediator.Send(new GetKenoBetHistoryRequest(Enumeration.FromValue<GameType>(game), roundId, nickname)));
        }
    }
}