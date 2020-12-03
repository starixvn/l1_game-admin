using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class PayoutViewModel
    {
        private const int MinChoices = 2;

        public string Name { get; set; }

        public GameType Type { get; set; }

        public double MinBet { get; set; }

        public double MaxBet { get; set; }

        public Dictionary<string, double> MaxBetChoices => BetChoiceBetSettings?.ToDictionary(x => x.Name, x => x.MaxBet);

        public Dictionary<string, double> Odds => BetChoiceOdds?.ToDictionary(x => x.Name, x => x.Odds);

        public Dictionary<string, Dictionary<string, double>> Icons
            => IconOdds == null
            ? new Dictionary<string, Dictionary<string, double>>()
            : IconOdds.ToDictionary(x => x.Index, x => x.Odds.ToDictionary(y => y.Count, y => y.Odds));

        public List<IconOddsViewModel> IconOdds { get; set; }

        public List<MaxBetChoiceViewModel> BetChoiceBetSettings { get; set; }

        public List<OddsViewModel> BetChoiceOdds { get; set; }

        public bool DisplayMaxBetChoices => MaxBetChoices?.Count > MinChoices;
    }

    public class IconOddsViewModel
    {
        public IconOddsViewModel()
        {
        }

        public IconOddsViewModel(string index, Dictionary<string, double> odds)
        {
            Index = index;
            Odds = odds.Select(x => new IconOddsItemViewModel(x.Key, x.Value)).ToList();
        }

        public string Index { get; set; }

        public List<IconOddsItemViewModel> Odds { get; set; }
    }

    public class IconOddsItemViewModel
    {
        public IconOddsItemViewModel()
        {
        }

        public IconOddsItemViewModel(string count, double odds)
        {
            Count = count;
            Odds = odds;
        }

        public string Count { get; set; }

        public double Odds { get; set; }
    }
}