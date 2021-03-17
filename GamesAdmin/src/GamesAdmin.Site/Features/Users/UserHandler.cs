using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Users.Requests;
using GamesAdmin.Site.Features.Users.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Users
{
    public class UserHandler
        : IRequestHandler<IndexRequest, IndexViewModel>
        , IRequestHandler<AddUserRequest, ResultBase>
        , IRequestHandler<ChangePasswordRequest, ResultBase>
    {
        private readonly IUserService userService;

        public UserHandler(IUserService userService)
        {
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

        public async Task<ResultBase> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var updated = await userService.ChangePassword(request.Username, request.Password, request.NewPassword);

            if (updated)
            {
                return new ResultBase(string.Empty);
            }

            return new ResultBase("Cannot update password.");
        }
    }
}
