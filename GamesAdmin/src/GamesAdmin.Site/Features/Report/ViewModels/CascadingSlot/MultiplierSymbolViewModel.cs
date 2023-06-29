namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class MultiplierSymbolViewModel
    {
        public byte Symbol { get; set; }
        public string XMultiplier { get; set; }
        public int Multiplier { get; set; }
        public bool IsMultiplierSymbol => XMultiplier != string.Empty;
    }
}
