using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features.BuyFreeSpins.Requests;
using GamesAdmin.Site.Features.BuyFreeSpins.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.BuyFreeSpins
{
    public interface IBuyFreeSpinService
    {
        Task<IEnumerable<BuyFreeSpinSetting>> GetAll();
    }

    public class BuyFreeSpinService : IBuyFreeSpinService
    {
        private readonly IMediator mediator;

        public BuyFreeSpinService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<IEnumerable<BuyFreeSpinSetting>> GetAll()
            => mediator.Send(new GetAllBuyFreeSpinsRequest());
    }
}