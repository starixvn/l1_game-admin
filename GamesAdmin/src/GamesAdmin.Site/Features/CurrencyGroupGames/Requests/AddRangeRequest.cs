using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Requests
{
  public class AddRangeRequest : IRequest<bool>
  {
    public AddRangeRequest(IEnumerable<CurrencyGroupGame> currencyGroupGames)
    {
      CurrencyGroupGames = currencyGroupGames;
    }

    public IEnumerable<CurrencyGroupGame> CurrencyGroupGames { get; }
  }
}
