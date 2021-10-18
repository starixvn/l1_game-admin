using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Site.Features.Currencies.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetAllPartnerHandler : Handler, IRequestHandler<GetAllPartnerRequest, IEnumerable<SelectListItem>>
    {
        public GetAllPartnerHandler(IMapper mapper, ICurrencyApi api) : base(mapper, api)
        {
        }

        public async Task<IEnumerable<SelectListItem>> Handle(GetAllPartnerRequest request, CancellationToken cancellationToken)
        {
            // TODO integrate with partner feature
            var partners = new List<Partner>
            {
                new Partner("1W_ID", "1W", "Key", "CasinoURL", "MobiURL", "MiniURL", "ApiURL"),
                new Partner("SBO_ID", "SBO", "Key", "CasinoURL", "MobiURL", "MiniURL", "ApiURL")
            };

            return Mapper.Map<IEnumerable<SelectListItem>>(partners);
        }
    }
}