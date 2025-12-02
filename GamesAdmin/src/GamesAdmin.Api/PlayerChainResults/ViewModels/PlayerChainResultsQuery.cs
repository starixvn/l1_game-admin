using System;

namespace GamesAdmin.Api.PlayerChainResults.ViewModels
{
    public class PlayerChainResultsQuery
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool ExcludeUUS { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}

