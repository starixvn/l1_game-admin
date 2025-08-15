using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.BuyFreeSpins.Requests
{
    public class GetAllBuyFreeSpinsRequest : IRequest<IEnumerable<BuyFreeSpinSetting>>
    {
    }
}