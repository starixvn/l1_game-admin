using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Users.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Users
{
    [Route("api/users")]
    public class UserController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAll()
           => (await mediator.Send(new GetAllRequest()));

        [HttpPost("add")]
        public Task<bool> Create(User user)
            => mediator.Send(new CreateRequest(user));

        [HttpPost("delete")]
        public Task<bool> Delete(User user)
          => mediator.Send(new DeleteRequest(user));

        [HttpGet("{username}")]
        public async Task<User> Get(string username)
           => (await mediator.Send(new GetByNameRequest(username)));

        [HttpPost("signin/")]
        public async Task<User> Get(string username, string password)
           => (await mediator.Send(new SignInRequest(username, password)));
    }
}
