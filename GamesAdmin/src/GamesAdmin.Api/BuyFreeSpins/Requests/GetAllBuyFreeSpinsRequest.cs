using System.Collections.Generic;
using GamesAdmin.Api.BuyFreeSpins.ViewModels;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.BuyFreeSpins.Requests
{
    public class GetAllBuyFreeSpinsRequest : IRequest<IEnumerable<BuyFreeSpinSettingViewModel>>
    {
    }
}