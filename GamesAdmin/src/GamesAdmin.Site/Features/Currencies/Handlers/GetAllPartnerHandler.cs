using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.Partners;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetAllPartnerHandler : Handler, IRequestHandler<GetAllPartnerRequest, IEnumerable<SelectListItem>>
    {
        private readonly IPartnerService partnerService;

        public GetAllPartnerHandler(IMapper mapper, ICurrencyApi api, IPartnerService partnerService) : base(mapper, api)
        {
            this.partnerService = partnerService;
        }

        public async Task<IEnumerable<SelectListItem>> Handle(GetAllPartnerRequest request, CancellationToken cancellationToken)
        {
            var partners = await partnerService.GetAll();

            return Mapper.Map<IEnumerable<SelectListItem>>(partners);
        }
    }
}