using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.ResultMonitor.Requests;
using GamesAdmin.Site.Features.ResultMonitor.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.ResultMonitor
{
    [Authorize]
    public class ResultMonitorController : Controller
    {
        private readonly IMediator mediator;

        public ResultMonitorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public IActionResult SlotResult(string gameConfigName)
        => View(new SlotResultViewModel(gameConfigName));

        [HttpPost]
        public Task<bool> SetSlotResult([Bind] SlotResultViewModel viewModel)
        => mediator.Send(new SetSlotResultRequest(
            viewModel.GameName,
            viewModel.Nickname,
            viewModel.Results.Trim().Split(',').Select(x => byte.Parse(x)).ToArray()));
    }
}