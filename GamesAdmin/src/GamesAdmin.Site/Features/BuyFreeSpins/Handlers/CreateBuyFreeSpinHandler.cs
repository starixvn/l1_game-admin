using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.BuyFreeSpins.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.BuyFreeSpins.Handlers
{
    public class CreateBuyFreeSpinHandler : BuyFreeSpinHandler, IRequestHandler<CreateBuyFreeSpinRequest, ResultBase>
    {
        public CreateBuyFreeSpinHandler(IMapper mapper, IBuyFreeSpinApi api) : base(mapper, api)
        {
        }

        public async Task<ResultBase> Handle(CreateBuyFreeSpinRequest request, CancellationToken cancellationToken)
        {
            var result = await Api.Create(request);

            return result ? new ResultBase(string.Empty) : new ResultBase("Cannot create new buy free spin setting.");
        }
    }
}