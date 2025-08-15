using GamesAdmin.Api.BuyFreeSpins.ViewModels;
using MediatR;

namespace GamesAdmin.Api.BuyFreeSpins.Requests
{
    public class GetBuyFreeSpinByGameIdRequest : IRequest<BuyFreeSpinSettingViewModel>
    {
        public int GameId { get; }

        public GetBuyFreeSpinByGameIdRequest(int gameId)
        {
            GameId = gameId;
        } 
    }
}