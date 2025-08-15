using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.BuyFreeSpins.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.BuyFreeSpins.Handlers
{
    public class UpdateBuyFreeSpinHandler : BuyFreeSpinHandler, IRequestHandler<UpdateBuyFreeSpinRequest, ResultBase>
    {
        public UpdateBuyFreeSpinHandler(IMapper mapper, IBuyFreeSpinApi api) : base(mapper, api)
        {
        }

        public async Task<ResultBase> Handle(UpdateBuyFreeSpinRequest request, CancellationToken cancellationToken)
        {
            var result = await Api.Update(request);

            return result ? new ResultBase(string.Empty) : new ResultBase("Cannot update buy free spin setting.");
        }
    }
}