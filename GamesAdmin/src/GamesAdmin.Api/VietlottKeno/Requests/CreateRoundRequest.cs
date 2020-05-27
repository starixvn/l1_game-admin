using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.VietlottKeno.Requests
{
    public class CreateRoundRequest : IRequest
    {
        public CreateRoundRequest(CreateVietlottKenoRound kenoRound)
        {
            KenoRound = kenoRound;
        }

        public CreateVietlottKenoRound KenoRound { get; }
    }
}