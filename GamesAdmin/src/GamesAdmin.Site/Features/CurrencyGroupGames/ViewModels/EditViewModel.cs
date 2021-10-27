using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels
{
    public class EditViewModel : ViewModelBase
    {
        public string GameId { get; set; }

        public string GroupId { get; set; }

        public string GameName { get; set; }

        public string PartnerName { get; set; }

        public bool Enabled { get; set; }

        public double MinBet { get; set; }

        public double MaxBet { get; set; }

        public Dictionary<string, double> MaxBetPerChoices { get; set; }

        public string[] Chips { get; set; }

        public string[] SiteExclusions { get; set; }

        public StakeLevel[] StakeLevels { get; set; }
    }
}