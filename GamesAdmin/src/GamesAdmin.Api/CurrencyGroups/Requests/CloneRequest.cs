using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroups.Requests
{
  public class CloneRequest : IRequest<CurrencyGroupModel>
  {
    public CloneRequest(CloneCurrencyGroupModel model)
    {
      Model = model;
    }

    public CloneCurrencyGroupModel Model { get; }
  }

  public class CloneCurrencyGroupModel
  {
    public string Name { get; set; }

    public string PartnerId { get; set; }

    public bool Enabled { get; set; }

    public bool FractionalOddsFormat { get; set; }

    public string SourceGroupId { get; set; }
  }
}
