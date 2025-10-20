using System.Collections.Generic;

namespace GamesAdmin.Site.Features.PlayerChainResultsManagement.ViewModels
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
