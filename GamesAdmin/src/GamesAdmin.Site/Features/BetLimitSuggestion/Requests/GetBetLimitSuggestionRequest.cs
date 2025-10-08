using GamesAdmin.Core.Models.BetLimitSuggestion;
using GamesAdmin.Site.Features.BetLimitSuggestion.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.BetLimitSuggestion.Requests
{
  public class GetBetLimitSuggestionRequest : IRequest<BetLimitSuggestionResult>
  {
    public GetBetLimitSuggestionRequest(GetBetLimitSuggestionModel model)
    {
      Model = model;
    }

    public GetBetLimitSuggestionModel Model { get; set; }
  }
}
