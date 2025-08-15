using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Api.BuyFreeSpins.Requests
{
    public class UpdateBuyFreeSpinModel : CreateBuyFreeSpinModel
    {
    }

    public class UpdateBuyFreeSpinRequest : CreateBuyFreeSpinRequest
    {
        public UpdateBuyFreeSpinRequest(bool enabled, int multiplier, BuyFreeSpinSiteExclusion[] siteExclusions, int gameId) : base(enabled, multiplier, siteExclusions, gameId)
        {
        }

    }
}