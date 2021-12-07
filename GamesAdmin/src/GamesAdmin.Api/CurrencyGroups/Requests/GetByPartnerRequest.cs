using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroups.Requests
{
    public class GetByPartnerRequest : IRequest<IEnumerable<CurrencyGroupModel>>
    {
        public string PartnerId { get; }

        public GetByPartnerRequest(string partnerId)
        {
            PartnerId = partnerId;
        }
    }
}