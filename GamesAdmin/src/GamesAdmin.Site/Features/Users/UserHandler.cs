using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Users.Requests;
using GamesAdmin.Site.Features.Users.ViewModels;
using MediatR;
using Sentry;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Users
{
    public class UserHandler
        : IRequestHandler<IndexRequest, IndexViewModel>
        , IRequestHandler<AddUserRequest, ResultBase>
    {
        private readonly ISentryClient sentryClient;
        private readonly IUserService userService;

        public UserHandler(
            ISentryClient sentryClient,
            IUserService userService)
        {
            this.sentryClient = sentryClient;
            this.userService = userService;
        }

        public async Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
        => new IndexViewModel { Users = (await userService.GetAll()).ToList() };

        public async Task<ResultBase> Handle(AddUserRequest request, CancellationToken cancellationToken)
        {
            var isExist = (await userService.Get(request.User.Username)) != null;

            if (isExist)
            {
                return new ResultBase("Username already exist.");
            }

            var added = await userService.Create(request.User);

            if (added)
            {
                return new ResultBase(string.Empty);
            }

            return new ResultBase("Cannot add new user");
        }
    }
}
