using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.BuyFreeSpins.Requests
{
    public class CreateBuyFreeSpinModel
    {
        public bool Enabled { get; set; }

        public int Multiplier { get; set; }

        public BuyFreeSpinSiteExclusion[] SiteExclusions { get; set; }

        public int GameId { get; set; }
    }

    public class CreateBuyFreeSpinRequest : IRequest<bool>
    {
        public CreateBuyFreeSpinRequest(bool enabled, int multiplier, BuyFreeSpinSiteExclusion[] siteExclusions, int gameId)
        {
            Enabled = enabled;
            Multiplier = multiplier;
            SiteExclusions = siteExclusions;
            GameId = gameId;
        }

        public bool Enabled { get; }

        public int Multiplier { get; }

        public BuyFreeSpinSiteExclusion[] SiteExclusions { get; }

        public int GameId { get; }
    }
}