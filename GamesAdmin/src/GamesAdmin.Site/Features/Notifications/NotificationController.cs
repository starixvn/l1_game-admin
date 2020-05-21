using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Notifications.Requests;
using GamesAdmin.Site.Features.Notifications.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Notifications
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly IMediator mediator;

        public NotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        => View(await mediator.Send(new IndexRequest()));

        [HttpGet]
        public IActionResult Add() => View(new AddViewModel(GenerateEventTypes(), GenerateDisplayKinds()));

        private static List<SelectListItem> GenerateEventTypes()
        => Enumeration.GetAll<EventScheduleType>().Select(x => new SelectListItem(x.DisplayName, x.Value)).ToList();

        private static List<SelectListItem> GenerateDisplayKinds()
        => Enumeration.GetAll<NotificationDisplayKind>().Select(x => new SelectListItem(x.DisplayName, x.Value)).ToList();

        [HttpPost]
        public async Task<IActionResult> Add([Bind]AddRequest addRequest)
        {
            var result = await mediator.Send(addRequest);

            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            return View();
        }
    }
}
