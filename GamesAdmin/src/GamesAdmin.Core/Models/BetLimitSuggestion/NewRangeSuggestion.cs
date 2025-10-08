using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetLimitSuggestion
{
  public class NewRangeSuggestion
  {
    public NewRangeSuggestion()
    {
      MaxBetPerChoiceGames = new List<CurrencyGroupGame>();
      CoinValueGames = new List<CurrencyGroupGame>();
      BetLevelGames = new List<CurrencyGroupGame>();
      NoSpecialConfigGames = new List<CurrencyGroupGame>();
    }

    public List<CurrencyGroupGame> MaxBetPerChoiceGames { get; set; }
    public List<CurrencyGroupGame> CoinValueGames { get; set; }
    public List<CurrencyGroupGame> BetLevelGames { get; set; }
    public List<CurrencyGroupGame> NoSpecialConfigGames { get; set; }
  }
}
