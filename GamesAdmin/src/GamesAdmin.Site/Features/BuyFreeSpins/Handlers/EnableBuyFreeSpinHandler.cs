using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.BuyFreeSpins.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.BuyFreeSpins.Handlers
{
    public class EnableBuyFreeSpinHandler : BuyFreeSpinHandler, IRequestHandler<EnableBuyFreeSpinRequest, ResultBase>
    {
        public EnableBuyFreeSpinHandler(IMapper mapper, IBuyFreeSpinApi api) : base(mapper, api)
        {
        }

        public async Task<ResultBase> Handle(EnableBuyFreeSpinRequest request, CancellationToken cancellationToken)
        {
            var result = await Api.Enable(request);

            return result ? new ResultBase(string.Empty) : new ResultBase("Cannot enable buy free spin setting.");
        }
    }
}