using System;
using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Requests
{
    public class GetCurrencyGroupsByPartnerRequest : IRequest<IEnumerable<CurrencyGroupModel>>
    {
        public GetCurrencyGroupsByPartnerRequest(string partnerId, Func<CurrencyGroupModel, bool> filter)
        {
            PartnerId = partnerId;
            Filter = filter;
        }

        public string PartnerId { get; }

        public Func<CurrencyGroupModel, bool> Filter { get;  }
    }
}