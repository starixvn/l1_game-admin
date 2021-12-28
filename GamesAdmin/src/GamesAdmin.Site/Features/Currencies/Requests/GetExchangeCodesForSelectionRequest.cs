using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.Requests
{
    public class GetExchangeCodesForSelectionRequest : IRequest<IEnumerable<SelectListItem>>

    {
    }
}