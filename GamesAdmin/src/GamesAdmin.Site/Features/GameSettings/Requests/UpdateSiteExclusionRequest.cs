using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateSiteExclusionRequest : IRequest<bool>
    {
        public string ConfigName { get; set; }

        public string SiteExclusions { get; set; }
    }
}