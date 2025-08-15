using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.BuyFreeSpins.Requests;
using GamesAdmin.Site.Features.BuyFreeSpins.ViewModels;
using GamesAdmin.Site.Features.Partners;
using MediatR;

namespace GamesAdmin.Site.Features.BuyFreeSpins.Handlers
{
    public class GetUpdateBuyFreeSpinPageHandler : BuyFreeSpinHandler, IRequestHandler<GetUpdateBuyFreeSpinPageRequest, UpdateBuyFreeSpinViewModel>
    {
        private readonly IPartnerService partnerService;
        public GetUpdateBuyFreeSpinPageHandler(IMapper mapper, IBuyFreeSpinApi api, IPartnerService partnerService) : base(mapper, api)
        {
            this.partnerService = partnerService;
        }

        public async Task<UpdateBuyFreeSpinViewModel> Handle(GetUpdateBuyFreeSpinPageRequest request, CancellationToken cancellationToken)
        {
            var setting = await Api.GetById(request.GameId);
            var operatorIds = (await partnerService.GetAll()).Select(partner => partner.PartnerId);

            return new UpdateBuyFreeSpinViewModel(operatorIds)
            {
                GameName = setting.GameName,
                GameId = setting.GameId,
                Multiplier = setting.Multiplier,
                Enabled = setting.Enabled,
                SiteExclusions = setting.SiteExclusions,
            };
        }
    }
}