using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.BuyFreeSpins.Requests
{
    public class EnableBuyFreeSpinModel
    {
        public int[] GameIds { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class EnableBuyFreeSpinRequest: IRequest<bool>
    {
        public EnableBuyFreeSpinRequest(int[] gameIds, bool isEnabled)
        {
            GameIds = gameIds;
            IsEnabled = isEnabled;
        }

        public int[] GameIds { get; }
        public bool IsEnabled { get; }
    }
}