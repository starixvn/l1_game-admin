using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.ProVietKeno.Requests
{
    public class CreateRoundRequest : IRequest
    {
        public CreateRoundRequest(CreateProVietKenoRound kenoRound)
        {
            KenoRound = kenoRound;
        }

        public CreateProVietKenoRound KenoRound { get; }
    }
}