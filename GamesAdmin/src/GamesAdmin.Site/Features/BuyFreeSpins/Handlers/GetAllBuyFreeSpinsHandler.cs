using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features.BuyFreeSpins.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.BuyFreeSpins.Handlers
{
    public class GetAllBuyFreeSpinsHandler : BuyFreeSpinHandler, IRequestHandler<GetAllBuyFreeSpinsRequest, IEnumerable<BuyFreeSpinSetting>>
    {
        public GetAllBuyFreeSpinsHandler(IMapper mapper, IBuyFreeSpinApi api) : base(mapper, api)
        {
        }

        public async Task<IEnumerable<BuyFreeSpinSetting>> Handle(GetAllBuyFreeSpinsRequest request, CancellationToken cancellationToken)
        {
            var a= await Api.GetAll();
            return a;
        }
            
    }
}