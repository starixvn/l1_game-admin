using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
  public class CurrencyGroupProposal
  {
    public double Rate { get; set; }
    public bool GetFromAllOperators { get; set; }
    public IEnumerable<string> OperatorIds { get; set; }
    public bool GetFromAllGames { get; set; }
    public IEnumerable<string> GameNames { get; set; }
    public string BaseCurrencyGroupId { get; set; }
  }
}