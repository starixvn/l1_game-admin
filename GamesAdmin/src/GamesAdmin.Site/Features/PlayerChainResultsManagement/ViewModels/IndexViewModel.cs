using System;

namespace GamesAdmin.Site.Features.PlayerChainResultsManagement.ViewModels
{
    public class IndexViewModel : PlayerChainResultsResponse
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool ExcludeUUS { get; set; }
    }
}
