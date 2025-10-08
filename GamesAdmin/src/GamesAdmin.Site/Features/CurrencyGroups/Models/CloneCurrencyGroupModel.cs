namespace GamesAdmin.Site.Features.CurrencyGroups.Models
{
  public class CloneCurrencyGroupModel
  {
    public string Name { get; set; }

    public string PartnerId { get; set; }

    public bool Enabled { get; set; }

    public bool FractionalOddsFormat { get; set; }

    public string SourceGroupId { get; set; }
  }
}