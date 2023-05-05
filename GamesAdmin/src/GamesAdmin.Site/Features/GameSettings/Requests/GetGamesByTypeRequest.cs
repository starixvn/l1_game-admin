using GamesAdmin.Core.Enumeration;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class GetGamesByTypeRequest : IRequest<IEnumerable<SelectListItem>>
    {
        public GetGamesByTypeRequest(GameType type)
        {
            Type = type;
        }

        public GameType Type { get; }
    }
}
