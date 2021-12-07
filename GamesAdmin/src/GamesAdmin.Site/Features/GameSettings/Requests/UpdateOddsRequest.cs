using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateOddsRequest : IRequest<bool>
    {
        public UpdateOddsRequest()
        {
        }

        public UpdateOddsRequest(
            string name,
            GameType type,
            Dictionary<string, double> odds,
            Dictionary<string, Dictionary<string, double>> icons)
        {
            Name = name;
            Type = type;
            Odds = odds;
            Icons = icons;
        }

        public string Name { get; set; }

        public GameType Type { get; set; }

        public Dictionary<string, double> Odds { get; set; }

        public Dictionary<string, Dictionary<string, double>> Icons { get; set; }
    }
}