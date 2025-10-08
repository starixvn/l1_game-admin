using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetLimitSuggestion
{
  public class CurrencyGroupProposal
  {
    public double RateToBase { get; set; }
    public IEnumerable<string> CandidateCurrencyGroupIds { get; set; }
    public string BaseCurrencyGroupId { get; set; }
  }
}