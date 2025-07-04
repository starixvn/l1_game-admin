namespace GamesAdmin.Core.Models
{
  public class NewCurrencySuggestion
  {
    public string PartnerId { get; set; }
    public double Rate { get; set; }
    public bool GetFromAllOperators { get; set; }
  }
}