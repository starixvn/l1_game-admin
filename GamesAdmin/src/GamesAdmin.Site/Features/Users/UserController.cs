using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.Users.Requests;
using GamesAdmin.Site.Features.Users.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        [HttpGet]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        public async Task<IActionResult> ChangePassword([Bind] ChangePasswordViewModel viewModel)
        {
            if (!viewModel.NewPassword.Equals(viewModel.ConfirmNewPassword))
            {
                viewModel.Success = false;
                viewModel.ErrorMessage = "Password and Confirm Password must match.";

                return View(viewModel);
            }
            
            var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
            var result = await mediator.Send(new ChangePasswordRequest(username, viewModel.Password, viewModel.NewPassword));

            viewModel.Success = result.Success;
            viewModel.ErrorMessage = result.Success ? string.Empty : result.Error;

            return View(viewModel);
        }
    }
}
