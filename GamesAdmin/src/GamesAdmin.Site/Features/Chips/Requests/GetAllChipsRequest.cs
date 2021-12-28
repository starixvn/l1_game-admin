using System.Collections.Generic;
using GamesAdmin.Core.Models.Chips;
using MediatR;

namespace GamesAdmin.Site.Features.Chips.Requests
{
    public class GetAllChipsRequest : IRequest<IEnumerable<ChipModel>>
    {
    }
}