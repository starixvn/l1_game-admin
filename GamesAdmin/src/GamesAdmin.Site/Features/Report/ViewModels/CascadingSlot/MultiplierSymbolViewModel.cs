namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class MultiplierSymbolViewModel
    {
        public byte Symbol { get; set; }
        public string Multiplier { get; set; }
        public bool IsMultiplierSymbol => Multiplier != string.Empty;
    }
}
