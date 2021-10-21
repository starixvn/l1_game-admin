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

        public Task<IEnumerable<SelectListItem>> Handle(GetPartnersForSelectionRequest request, CancellationToken cancellationToken)
              => partnerService.GetPartnersForSelection(partner => true);
    }
}