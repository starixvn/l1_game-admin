using GamesAdmin.Site.Features.GameUsers.Requests;
using GamesAdmin.Site.Features.GameUsers.Results;
using GamesAdmin.Site.Features.GameUsers.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.GameUsers
{
    public class GameUserHandler
        : IRequestHandler<AddRequest, AddViewModel>
        , IRequestHandler<AddUsersRequest, List<AddUsersResult>>
        , IRequestHandler<ReviseRequest, int>
        , IRequestHandler<GetSpinRequest, MemberSpinViewModel>
    {
        private readonly IGameUserService gameUserService;

        public GameUserHandler(IGameUserService gameUserService)
        {
            this.gameUserService = gameUserService;
        }

        public Task<AddViewModel> Handle(AddRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new AddViewModel());

        public Task<List<AddUsersResult>> Handle(AddUsersRequest request, CancellationToken cancellationToken)
        {
            var names = request.TextNames?
                .Split(';')
                .Select(name => name.Trim())
                .ToArray();

            if (names?.Length > 0)
            {
                return gameUserService.AddUsers(names, request.IsBot);
            }

            return Task.FromResult(new List<AddUsersResult>());
        }

        public Task<int> Handle(ReviseRequest request, CancellationToken cancellationToken)
        => gameUserService.ReviseBots();

        public async Task<MemberSpinViewModel> Handle(GetSpinRequest request, CancellationToken cancellationToken)
        {
            var spins = await gameUserService.GetMemberSpins(request.MemberId);
            var logs = await gameUserService.GetMemberSpinLogs(request.MemberId);

            return new MemberSpinViewModel(spins, logs);
        }
    }
}
