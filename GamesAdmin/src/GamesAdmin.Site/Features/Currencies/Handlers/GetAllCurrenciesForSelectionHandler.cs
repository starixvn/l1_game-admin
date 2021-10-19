using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Currencies.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetAllCurrenciesForSelectionHandler
        : Handler, IRequestHandler<GetCurrenciesForSelectionRequest, IEnumerable<SelectListItem>>
    {
        public GetAllCurrenciesForSelectionHandler(IMapper mapper, ICurrencyApi api) : base(mapper, api)
        {
        }

        public async Task<IEnumerable<SelectListItem>> Handle(GetCurrenciesForSelectionRequest request, CancellationToken cancellationToken)
        {
            var models = await Api.GetAll();
            var filteredModels = models.Where(request.Criteria);

            return filteredModels.Select(model => Mapper.Map<SelectListItem>(model));
        }
    }
}