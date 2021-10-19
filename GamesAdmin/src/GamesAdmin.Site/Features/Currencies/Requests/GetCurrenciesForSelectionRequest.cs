using System;
using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.Requests
{
    public class GetCurrenciesForSelectionRequest : IRequest<IEnumerable<SelectListItem>>
    {
        public GetCurrenciesForSelectionRequest(Func<CurrencyModel, bool> criteria)
        {
            Criteria = criteria;
        }

        public Func<CurrencyModel, bool> Criteria { get; set; }
    }
}