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
    public class GetPartnersForSelectionHandler : Handler, IRequestHandler<GetPartnersForSelectionRequest, IEnumerable<SelectListItem>>
    {
        private readonly IPartnerService partnerService;

        public GetPartnersForSelectionHandler(IMapper mapper, ICurrencyApi api, IPartnerService partnerService) : base(mapper, api)
        {
            this.partnerService = partnerService;
        }

        public async Task<IEnumerable<SelectListItem>> Handle(GetPartnersForSelectionRequest request, CancellationToken cancellationToken)
        {
            var partners = await partnerService.GetAll();

            return Mapper.Map<IEnumerable<SelectListItem>>(partners);
        }
    }
}