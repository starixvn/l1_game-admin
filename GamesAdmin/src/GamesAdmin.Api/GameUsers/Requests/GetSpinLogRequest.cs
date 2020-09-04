using System.Collections.Generic;
using GamesAdmin.Database.Entities.Spins;
using MediatR;

namespace GamesAdmin.Api.GameUser.Requests
{
    public class GetSpinLogRequest: IRequest<IList<SpinLogEntity>>
    {
        public GetSpinLogRequest(int memberId)
        {
            MemberId = memberId;
        }

        public int MemberId { get; }
    }
}
