using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateOddsRequest : IRequest<UpdateOddsResultViewModel>
    {
        public UpdateOddsRequest()
        {
        }

        public UpdateOddsRequest(
            string name,
            GameType type,
            Dictionary<string, double> odds,
            SlotsConfig slotsConfig)
        {
            Name = name;
            Type = type;
            Odds = odds;
            SlotsConfig = slotsConfig;
        }

        public string Name { get; set; }

        public GameType Type { get; set; }

        public Dictionary<string, double> Odds { get; set; }

        public SlotsConfig SlotsConfig { get; set; }
    }
}