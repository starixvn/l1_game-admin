
using GamesAdmin.Site.Features._Shared;
using MediatR;

namespace GamesAdmin.Site.Features.BuyFreeSpins.Requests
{
    public class EnableBuyFreeSpinRequest : IRequest<ResultBase>
    {
        public int[] GameIds { get; set; }
        public bool IsEnabled { get; set; }
    }
}