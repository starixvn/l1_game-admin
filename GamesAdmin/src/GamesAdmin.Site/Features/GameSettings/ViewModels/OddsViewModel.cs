using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class OddsViewModel
    {
        private const double MINIMUM_ODDS_VALUE = 0.01;

        public OddsViewModel() { }

        public OddsViewModel(string name, double odds, string displayName)
        {
            Name = name;
            Odds = odds;
            DisplayName = displayName;
        }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        [Required]
        [Range(MINIMUM_ODDS_VALUE, int.MaxValue)]
        public double Odds { get; set; }
    }
}
