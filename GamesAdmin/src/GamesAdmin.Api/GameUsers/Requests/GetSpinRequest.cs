using GamesAdmin.Database.Entities.Spins;
using MediatR;

namespace GamesAdmin.Api.GameUser.Requests
{
    public class GetSpinRequest: IRequest<SpinEntity>
    {
        public GetSpinRequest(int memberId)
        {
            MemberId = memberId;
        }

        public int MemberId { get; }
    }
}
