using System.Collections.Generic;

namespace GamesAdmin.Api.PlayerChainResults.ViewModels
{
    public class PlayerChainResultsResponse
    {
        public IEnumerable<PlayerChainResultViewModel> Results { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
