using GamesAdmin.Api.Users.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Users
{
    public class UserHandler
        : IRequestHandler<CreateRequest, bool>
        , IRequestHandler<GetAllRequest, IEnumerable<User>>
        , IRequestHandler<DeleteRequest, bool>
        , IRequestHandler<GetByNameRequest, User>
        , IRequestHandler<SignInRequest, User>
    {
        private readonly IUserService userService;

        public UserHandler(IUserService userService) 
        {
            this.userService = userService;
        }

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => userService.Create(request.User);

        public Task<IEnumerable<User>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        => userService.GetAll();

        public Task<bool> Handle(DeleteRequest request, CancellationToken cancellationToken)
        => userService.Delete(request.User);

        public Task<User> Handle(GetByNameRequest request, CancellationToken cancellationToken)
        => userService.Get(request.Username);

        public Task<User> Handle(SignInRequest request, CancellationToken cancellationToken)
        => userService.SignIn(request.Username, request.Password);
    }
}
