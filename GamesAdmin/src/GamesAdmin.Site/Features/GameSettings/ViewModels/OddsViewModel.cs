using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class OddsViewModel
    {
        public OddsViewModel() { }

        public OddsViewModel(string name, double odds, string displayName)
        {
            Name = name;
            Odds = odds;
            DisplayName = displayName;
        }

        public string Name { get; set; }

        public string DisplayName { get; }

        [Required]
        public double Odds { get; set; }
    }
}
