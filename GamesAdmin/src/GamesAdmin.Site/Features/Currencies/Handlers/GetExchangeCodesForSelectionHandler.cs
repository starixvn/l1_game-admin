using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.GameSettings;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetExchangeCodesForSelectionHandler : Handler, IRequestHandler<GetExchangeCodesForSelectionRequest, IEnumerable<SelectListItem>>
    {
        private readonly IGlobalConfigApi globalConfigApi;

        public GetExchangeCodesForSelectionHandler(IMapper mapper, ICurrencyApi api, IGlobalConfigApi globalConfigApi) : base(mapper, api)
        {
            this.globalConfigApi = globalConfigApi;
        }

        public async Task<IEnumerable<SelectListItem>> Handle(GetExchangeCodesForSelectionRequest request, CancellationToken cancellationToken)
        {
            var global = await globalConfigApi.GetExchangeCodes();

            var success = global.Configs.TryGetValue("codes", out IList<string> codes);

            if (!success)
            {
                codes = new List<string>();
            }

            return codes.Select(code => new SelectListItem(code, code));
        }
    }
}