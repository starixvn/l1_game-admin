using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Requests
{
    public class GetCurrencyGroupsByPartnerRequest : IRequest<IEnumerable<CurrencyGroupModel>>
    {
        public GetCurrencyGroupsByPartnerRequest(string partnerId)
        {
            PartnerId = partnerId;
        }

        public string PartnerId { get; }
    }
}