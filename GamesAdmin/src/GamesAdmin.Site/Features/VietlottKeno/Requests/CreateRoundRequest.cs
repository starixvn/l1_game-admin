using GamesAdmin.Site.Features.VietlottKeno.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.VietlottKeno.Requests
{
    public class CreateRoundRequest : IRequest
    {
        public CreateRoundRequest(CreateVietlottKenoRoundViewModel kenoRound)
        {
            KenoRound = kenoRound;
        }

        public CreateVietlottKenoRoundViewModel KenoRound { get; }
    }
}