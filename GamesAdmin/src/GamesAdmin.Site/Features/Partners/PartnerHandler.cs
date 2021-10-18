
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
    {
        private readonly IPartnerService partnerService;
        private readonly IMapper mapper;

        public PartnerHandler(IPartnerService partnerService, IMapper mapper)
        {
            this.partnerService = partnerService;
            this.mapper = mapper;
        }

        public async Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
            => new IndexViewModel { Partners = (await partnerService.GetAll()).ToList() };

        public async Task<ResultBase> Handle(AddPartnerRequest request, CancellationToken cancellationToken)
            => await partnerService.Add(request.PartnerInfo);

        public async Task<ResultBase> Handle(UpdateRequest request, CancellationToken cancellationToken)
            => await partnerService.Update(new Core.Models.Partners.Partner(request.PartnerId, request.Name, request.SecretKey, request.CasinoVersionUrl, request.MobiVersionUrl, request.MiniVersionUrl, request.ApiUrl));

        public async Task<PartnerEditViewModel> Handle(GetByIdRequest request, CancellationToken cancellationToken)
            => mapper.Map<PartnerEditViewModel>(await partnerService.GetById(request.PartnerId));
    }
}
