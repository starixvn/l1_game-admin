using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetLimitSuggestion
{
  public class BetLimitSuggestionResult
  {
    public CurrencyGroupSuggestion BestSuggestion { get; set; }
    public CurrencyGroupSuggestion BestSmallerSuggestion { get; set; }
  }

  public class CurrencyGroupSuggestion
  {
    public string GroupId { get; set; }
    public string GroupName { get; set; }
    public string PartnerId { get; set; }
    public double Rate { get; set; }
    public double ValueDifference { get; set; }
    public bool IsSmallerThanExpected { get; set; }
    public List<GameBetLimitComparison> GameComparisons { get; set; } = new List<GameBetLimitComparison>();
  }

  public class GameBetLimitComparison
  {
    public int GameId { get; set; }
    public string GameName { get; set; }
    public string GameDisplayName { get; set; }
    public BetLimitComparison MinBetComparison { get; set; }
    public BetLimitComparison MaxBetComparison { get; set; }
    public BetLimitComparison MaxBetPerChoiceComparison { get; set; }
    public BetLimitComparison MinCoinValueComparison { get; set; }
    public BetLimitComparison MaxCoinValueComparison { get; set; }
    public BetLimitComparison MinBetLevelComparison { get; set; }
    public BetLimitComparison MaxBetLevelComparison { get; set; }
  }

  public class BetLimitComparison
  {
    public double BaseValue { get; set; }
    public double CandidateValue { get; set; }
    public double ExpectedValue { get; set; }
    public double Difference { get; set; }
    public double DifferencePercentage { get; set; }
    public bool IsSmallerThanExpected { get; set; }
  }
}