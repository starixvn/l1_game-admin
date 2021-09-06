namespace GamesAdmin.Site.Features.ClientSites.ViewModels
{
    public class ClientSiteViewModel
    {
        public string ClientId { get; set; }

        public string SiteId { get; set; }

        public string GameClientUrl { get; set; }

        public string GameMobiClientUrl { get; set; }

        public string MiniSabaGameClientUrl { get; set; }

        public string ValidCurrencies { get; set; }
    }
}