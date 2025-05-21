using System.Collections.Generic;
using GamesAdmin.Site.Features.JackpotSettingsManagement.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.JackpotSettingsManagement.Requests
{
    public class SearchRequest : IRequest<IEnumerable<SearchResultViewModel>>
    {
        public IEnumerable<int> GameIds { get; set; }
        public string SiteNames { get; set; }
    }
}
