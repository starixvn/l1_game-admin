using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.Requests
{
    public class GetPartnersForSelectionRequest : IRequest<IEnumerable<SelectListItem>>
    {
    }
}