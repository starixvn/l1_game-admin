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

        public string DisplayName { get; set; }

        [Required]
        [Range(0.1, int.MaxValue)]
        public double Odds { get; set; }
    }
}
