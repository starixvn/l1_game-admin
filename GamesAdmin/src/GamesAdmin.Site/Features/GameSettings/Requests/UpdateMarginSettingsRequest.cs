using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateMarginSettingsRequest : IRequest<string>
    {
        public UpdateMarginSettingsRequest(
            string gameName,
            IEnumerable<Dictionary<string, double>> marginSettings)
        {
            GameName = gameName;
            MarginSettings = marginSettings;
        }

        public string GameName { get; set; }

        public IEnumerable<Dictionary<string, double>> MarginSettings { get; set; }
    }
}