using GamesAdmin.Site.Features.BuyFreeSpins.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.BuyFreeSpins.Requests
{
    public class GetUpdateBuyFreeSpinPageRequest : IRequest<UpdateBuyFreeSpinViewModel>
    {
        public int GameId { get; }

        public GetUpdateBuyFreeSpinPageRequest(int gameId)
        {
            GameId = gameId;
        }
    }
}

