using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class ClientSite
    {
        public string ClientId { get; set; }

        public string SiteId { get; set; }

        public string GameClientUrl { get; set; }

        public string GameMobiClientUrl { get; set; }

        public string MiniSabaGameClientUrl { get; set; }

        public IEnumerable<string> ValidCurrencies { get; set; }
    }
}