using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.JackpotSettingsRequests
    {
    public class SearchRequest : IRequest<IEnumerable<JackpotSettings>>
    {
        public IEnumerable<int> GameIds { get; set; }
        public IEnumerable<string> SiteNames { get; set; }
    }
}
