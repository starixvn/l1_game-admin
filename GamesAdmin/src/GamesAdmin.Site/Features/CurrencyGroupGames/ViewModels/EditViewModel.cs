using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features._Shared;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels
{
    public class EditViewModel : ViewModelBase
    {
        public string GameId { get; set; }

        public string GroupId { get; set; }

        public string GameName { get; set; }

        public string PartnerName { get; set; }

        public IEnumerable<CurrencyGroupModel> CurrencyGroups { get; set; }

        public bool Enabled { get; set; }

        public double Minbet { get; set; }

        public double Maxbet { get; set; }

        public Dictionary<string, double> MaxBetPerChoices { get; set; }

        public string[] Chips { get; set; }

        public string[] SiteExclusions { get; set; }
    }
}