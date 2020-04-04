using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.Users.Requests;
using GamesAdmin.Site.Features.Users.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Users
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View(await mediator.Send(new IndexRequest()));

        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add([Bind] AddViewModel addViewModel)
        {
            if (!addViewModel.Password.Equals(addViewModel.ConfirmPassword))
            {
                addViewModel.Success = false;
                addViewModel.ErrorMessage = "Password and Confirm Password must match.";

                return View(addViewModel);
            }

            var addedResult = await mediator.Send(new AddUserRequest(
                new User(
                    addViewModel.Username,
                    addViewModel.Password,
                    addViewModel.IsAdmin)));

            addViewModel.Success = addedResult.Success;

            if (addViewModel.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                addViewModel.ErrorMessage = addedResult.Error;
            }

            return View(addViewModel);
        }
    }
}
