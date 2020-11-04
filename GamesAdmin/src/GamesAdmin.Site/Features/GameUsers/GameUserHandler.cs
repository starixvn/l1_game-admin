using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.GameUsers.Requests;
using GamesAdmin.Site.Features.GameUsers.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameUsers
{
    public class GameUserHandler : IRequestHandler<GetSpinRequest, MemberSpinViewModel>
    {
        private readonly IGameUserService gameUserService;

        public GameUserHandler(IGameUserService gameUserService)
        {
            this.gameUserService = gameUserService;
        }

        public async Task<MemberSpinViewModel> Handle(GetSpinRequest request, CancellationToken cancellationToken)
        {
            var spins = await gameUserService.GetMemberSpins(request.MemberId);
            var logs = await gameUserService.GetMemberSpinLogs(request.MemberId);

            return new MemberSpinViewModel(spins, logs);
        }
    }
}
