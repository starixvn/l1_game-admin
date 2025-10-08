using System;
using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Requests
{
  public class GetCurrencyGroupByIdsRequest : IRequest<IEnumerable<CurrencyGroupModel>>
  {
    public GetCurrencyGroupByIdsRequest(IEnumerable<string> currencyGroupIds)
    {
      CurrencyGroupIds = currencyGroupIds;
    }

    public IEnumerable<string> CurrencyGroupIds { get; }
  }
}