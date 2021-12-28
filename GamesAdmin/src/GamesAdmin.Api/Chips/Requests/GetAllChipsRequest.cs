using System.Collections.Generic;
using GamesAdmin.Core.Models.Chips;
using MediatR;

namespace GamesAdmin.Api.Chips.Requests
{
    public class GetAllChipsRequest : IRequest<IEnumerable<ChipModel>>
    {
    }
}