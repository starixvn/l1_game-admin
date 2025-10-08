using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetLimitSuggestion;
using GamesAdmin.Site.Features.BetLimitSuggestion.ViewModels;

namespace GamesAdmin.Site.Features.BetLimitSuggestion
{
  public class BetLimitSuggestionService
  {
    private readonly IBetLimitSuggestionApi betLimitSuggestionApi;

    public BetLimitSuggestionService(IBetLimitSuggestionApi betLimitSuggestionApi)
    {
      this.betLimitSuggestionApi = betLimitSuggestionApi;
    }
  }
}