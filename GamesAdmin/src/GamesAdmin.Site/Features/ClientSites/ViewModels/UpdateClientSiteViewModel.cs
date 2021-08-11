using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.ClientSites.ViewModels
{
    public class UpdateClientSiteViewModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string SiteId { get; set; }

        [Required]
        public string GameClientUrl { get; set; }

        public string GameMobiClientUrl { get; set; }

        public string MiniSabaGameClientUrl { get; set; }

        public string ValidCurrencies { get; set; }
    }
}