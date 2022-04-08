using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.Currencies.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetCurrenciesForSelectionHandler
        : Handler, IRequestHandler<GetCurrenciesForSelectionRequest, IEnumerable<SelectListItem>>
    {
        public GetCurrenciesForSelectionHandler(IMapper mapper, ICurrencyApi api) : base(mapper, api)
        {
        }

        public async Task<IEnumerable<SelectListItem>> Handle(GetCurrenciesForSelectionRequest request, CancellationToken cancellationToken)
        {
            Func<CurrencyModel, SelectListItem> defaultTransform = model => Mapper.Map<SelectListItem>(model);

            var models = await Api.GetAll();
            var filteredModels = models.Where(request.Criteria);
            var transform = request.Transform ?? defaultTransform;

            return filteredModels.Select(transform).ToList();
        }
    }
}