using System;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using MediatR;

namespace GamesAdmin.Site.Features.BuyFreeSpins.Requests
{
    public class CreateBuyFreeSpinRequest : IRequest<ResultBase>
    {
        public bool Enabled { get; set; }

        public int Multiplier { get; set; }

        public BuyFreeSpinSiteExclusion[] SiteExclusions { get; set; } = Array.Empty<BuyFreeSpinSiteExclusion>();

        public int GameId { get; set; }

    }
}