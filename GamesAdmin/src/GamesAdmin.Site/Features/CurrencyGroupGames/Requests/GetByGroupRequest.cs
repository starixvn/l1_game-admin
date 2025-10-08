using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Requests
{
  public class GetByGroupRequest : IRequest<IEnumerable<CurrencyGroupGame>>
  {
    public GetByGroupRequest(string groupId)
    {
      GroupId = groupId;
    }

    public string GroupId { get; }
  }
}
