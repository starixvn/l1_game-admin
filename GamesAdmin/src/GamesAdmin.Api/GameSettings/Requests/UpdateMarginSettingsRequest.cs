using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateMarginSettingsRequest : IRequest<bool>
    {
        public UpdateMarginSettingsRequest(string gameName, IEnumerable<Dictionary<string, double>> marginSettings)
        {
            GameName = gameName;
            MarginSettings = marginSettings;
        }

        public string GameName { get; }

        public IEnumerable<Dictionary<string, double>> MarginSettings { get; }
    }
}