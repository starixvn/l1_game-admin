using GamesAdmin.Api.Users.Requests;
using GamesAdmin.Api.Users.Results;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Users
{
    public class UserHandler
        : IRequestHandler<GetBotCountRequest, int>
        , IRequestHandler<GenerateUserRandomRequest, IList<GenerateUserResult>>
    {
        private readonly IUserService userService;

        public UserHandler(IUserService userService) 
        {
            this.userService = userService;
        }

        public Task<int> Handle(GetBotCountRequest request, CancellationToken cancellationToken)
        => userService.GetTotalBots();

        public Task<IList<GenerateUserResult>> Handle(GenerateUserRandomRequest request, CancellationToken cancellationToken)
        => userService.GenerateUsers(request.Names, request.IsBot);
    }
}
