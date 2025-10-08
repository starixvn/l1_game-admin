using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.CurrencyGroups.Models;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Requests
{
  public class CloneCurrencyGroupRequest : IRequest<CurrencyGroupModel>
  {
    public CloneCurrencyGroupRequest(CloneCurrencyGroupModel model)
    {
      Model = model;
    }

    public CloneCurrencyGroupModel Model { get; }
  }
}
