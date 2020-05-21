using System.Threading.Tasks;
using GamesAdmin.Site.Features.Notifications.Requests;
using GamesAdmin.Site.Features.Notifications.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
