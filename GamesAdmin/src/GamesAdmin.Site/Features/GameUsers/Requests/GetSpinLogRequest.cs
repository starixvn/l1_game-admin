using System.Collections.Generic;
using GamesAdmin.Core.Models.Spins;
using MediatR;

namespace GamesAdmin.Site.Features.GameUsers.Requests
{
    public class GetSpinLogRequest : IRequest<IList<SpinLog>>
    {
        public GetSpinLogRequest(int memberId)
        {
            MemberId = memberId;
        }

        public int MemberId { get; }
    }
}
