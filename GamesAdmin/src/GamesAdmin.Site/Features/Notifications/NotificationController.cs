using System.Threading.Tasks;
using Audit.Mvc;
using GamesAdmin.Site.Features.Notifications.Requests;
using GamesAdmin.Site.Features.Notifications.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

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
        public IActionResult Add() => View(new AddViewModel());

        [HttpPost]
        public async Task<IActionResult> Add([Bind] AddRequest addRequest)
        {
            var result = await mediator.Send(addRequest);

            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Add));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        => View(await mediator.Send(new GetByIdRequest(id)));

        [HttpPost, Audit]
        public async Task<IActionResult> Edit([Bind] UpdateRequest request)
        {
            var result = await mediator.Send(request);

            return result
                ? RedirectToAction(nameof(Index))
                : RedirectToAction(nameof(Edit), new RouteValueDictionary
                    {
                        { "id", request.Id }
                    });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await mediator.Send(new DeleteRequest(id));

            return Json(result);
        }
    }
}
