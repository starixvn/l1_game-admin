using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site._Shared.Services.GeneralGameSettings;
using GamesAdmin.Site.Features.BuyFreeSpins.Requests;
using GamesAdmin.Site.Features.BuyFreeSpins.ViewModels;
using GamesAdmin.Site.Features.Partners;
using MediatR;

namespace GamesAdmin.Site.Features.BuyFreeSpins.Handlers
{
    public class GetCreateBuyFreeSpinPageHandler : BuyFreeSpinHandler, IRequestHandler<GetCreateBuyFreeSpinPageRequest, CreateBuyFreeSpinViewModel>
    {
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly IPartnerService partnerService;
        public GetCreateBuyFreeSpinPageHandler(IMapper mapper, IBuyFreeSpinApi api, IGeneralGameSettingService generalGameSettingService, IPartnerService partnerService) : base(mapper, api)
        {
            this.generalGameSettingService = generalGameSettingService;
            this.partnerService = partnerService;
        }

        public async Task<CreateBuyFreeSpinViewModel> Handle(GetCreateBuyFreeSpinPageRequest request, CancellationToken cancellationToken)
        {
            var allSlotGames = (await generalGameSettingService.GetAllGames()).Where(game => game.Type == GameType.CascadingSlots);
            var operatorIds = (await partnerService.GetAll()).Select(partner => partner.PartnerId);
            return new CreateBuyFreeSpinViewModel(allSlotGames, operatorIds);
        }
    }
}