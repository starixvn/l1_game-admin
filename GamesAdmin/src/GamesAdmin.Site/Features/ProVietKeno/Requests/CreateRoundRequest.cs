using GamesAdmin.Site.Features.ProVietKeno.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ProVietKeno.Requests
{
    public class CreateRoundRequest : IRequest
    {
        public CreateRoundRequest(CreateProVietKenoRoundViewModel kenoRound)
        {
            KenoRound = kenoRound;
        }

        public CreateProVietKenoRoundViewModel KenoRound { get; }
    }
}