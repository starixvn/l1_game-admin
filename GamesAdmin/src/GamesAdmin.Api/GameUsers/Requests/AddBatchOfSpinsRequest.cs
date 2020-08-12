using System.Collections.Generic;
using GamesAdmin.Api.GameUser.Requests;
using GamesAdmin.Api.GameUsers.Results;
using MediatR;

namespace GamesAdmin.Api.GameUsers.Requests
{
    public class AddBatchOfSpinsRequest : IRequest<AddBatchOfSpinResult>
    {
        public AddBatchOfSpinsRequest(IList<AddSpinRequest> spins)
        {
            Spins = spins;
        }

        public IList<AddSpinRequest> Spins { get; }
    }
}
