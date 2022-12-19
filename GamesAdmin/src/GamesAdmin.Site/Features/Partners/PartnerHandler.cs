using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Partners.Requests;
using GamesAdmin.Site.Features.Partners.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Partners
{
    public class PartnerHandler
        : IRequestHandler<IndexRequest, IndexViewModel>
        , IRequestHandler<AddPartnerRequest, ResultBase>
        , IRequestHandler<UpdateRequest, ResultBase>
        , IRequestHandler<GetByIdRequest, PartnerEditViewModel>
        , IRequestHandler<GetSiteExclusionRequest, string[]>
        , IRequestHandler<UpdateSiteExclusionRequest, bool>
        , IRequestHandler<UpdatePartnerUMRequest, bool>
        , IRequestHandler<UpdateSuperPlayerRequest, bool>
        , IRequestHandler<GetSuperPlayerViewRequest, SuperPlayersViewModel>
        , IRequestHandler<GetSuperPlayerRequest, string>
    {
        private readonly IPartnerService partnerService;
        private readonly IMapper mapper;

        public PartnerHandler(IPartnerService partnerService, IMapper mapper)
        {
            this.partnerService = partnerService;
            this.mapper = mapper;
        }

        public async Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
            => new IndexViewModel { Partners = (await partnerService.GetAll()).ToList(), UmInfos = (await partnerService.GetAllUMInfo()).ToList() };

        public async Task<ResultBase> Handle(AddPartnerRequest request, CancellationToken cancellationToken)
            => await partnerService.Add(request.PartnerInfo);

        public async Task<ResultBase> Handle(UpdateRequest request, CancellationToken cancellationToken)
            => await partnerService.Update(new Partner(request.PartnerId, request.Name, request.SecretKey, request.ClubVersionUrl, request.MobiVersionUrl, request.MiniVersionUrl, request.MegaKenoUrl, request.TestCurrencyUrl, request.IsDisplayPlayerCurrency));

        public async Task<PartnerEditViewModel> Handle(GetByIdRequest request, CancellationToken cancellationToken)
            => mapper.Map<PartnerEditViewModel>(await partnerService.GetById(request.PartnerId));

        public Task<string[]> Handle(GetSiteExclusionRequest request, CancellationToken cancellationToken)
            => partnerService.GetPartnerSiteExclusion(request.PartnerId, request.GameId);

        public Task<bool> Handle(UpdateSiteExclusionRequest request, CancellationToken cancellationToken)
            => partnerService.UpdatePartnerSiteExclusion(request.PartnerId, request.GameId, request.ExclusionSites);

        public Task<bool> Handle(UpdatePartnerUMRequest request, CancellationToken cancellationToken)
            => partnerService.UpdatePartnerUM(request);

        public async Task<SuperPlayersViewModel> Handle(GetSuperPlayerViewRequest request, CancellationToken cancellationToken)
        {
            var partners = await partnerService.GetAll();

            return new SuperPlayersViewModel(partners);
        }

        public Task<bool> Handle(UpdateSuperPlayerRequest request, CancellationToken cancellationToken)
            => partnerService.UpdatePartnerSuperPlayer(request.PartnerId, request.SuperPlayers);

        public async Task<string> Handle(GetSuperPlayerRequest request, CancellationToken cancellationToken)
        {
            var superPlayers = await partnerService.GetPartnerSuperPlayer(request.PartnerId);

            return string.Join("; ", superPlayers);
        }
    }
}