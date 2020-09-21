using GamesAdmin.Site.Features.GameUsers.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameUsers.Requests
{
    public class GetSpinRequest : IRequest<MemberSpinViewModel>
    {
        public GetSpinRequest(int memberId) 
        {
            MemberId = memberId;
        }

        public int MemberId { get; }
    }
}
