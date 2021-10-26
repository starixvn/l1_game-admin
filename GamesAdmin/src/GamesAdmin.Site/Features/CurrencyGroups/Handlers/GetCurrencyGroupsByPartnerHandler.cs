using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
    public class GetCurrencyGroupsByPartnerHandler : Handler, IRequestHandler<GetCurrencyGroupsByPartnerRequest, IEnumerable<CurrencyGroupModel>>
    {
        public GetCurrencyGroupsByPartnerHandler(IMapper mapper, ICurrencyGroupApi api) : base(mapper, api)
        {
        }

        public Task<IEnumerable<CurrencyGroupModel>> Handle(GetCurrencyGroupsByPartnerRequest request, CancellationToken cancellationToken)
            => Api.GetByPartner(request.PartnerId);
    }
}