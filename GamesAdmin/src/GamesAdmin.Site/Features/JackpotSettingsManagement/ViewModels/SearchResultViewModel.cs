using System.Collections.Generic;

namespace GamesAdmin.Site.Features.JackpotSettingsManagement.ViewModels
{
    public class SearchResultViewModel
    {
        public string Id { get; set; }
        public bool Enabled { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
        public IEnumerable<string> SiteNames { get; set; }
        public bool BotEnabled { get; set; }
    }
}