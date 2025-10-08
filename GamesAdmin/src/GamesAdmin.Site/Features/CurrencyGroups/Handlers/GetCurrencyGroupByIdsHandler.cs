using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
  public class GetCurrencyGroupByIdsHandler : Handler, IRequestHandler<GetCurrencyGroupByIdsRequest, IEnumerable<CurrencyGroupModel>>
  {
    public GetCurrencyGroupByIdsHandler(IMapper mapper, ICurrencyGroupApi api) : base(mapper, api)
    {
    }

    public async Task<IEnumerable<CurrencyGroupModel>> Handle(GetCurrencyGroupByIdsRequest request, CancellationToken cancellationToken)
    {
      var groupIds = request.CurrencyGroupIds.Where(id => !string.IsNullOrWhiteSpace(id)).ToList();

      // If no valid group IDs, return empty list
      if (!groupIds.Any())
      {
        return new List<CurrencyGroupModel>();
      }

      var queryIds = string.Join(',', groupIds);
      var groups = await Api.Query(queryIds);
      return groups;
    }
  }
}