using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class MaxBetChoiceViewModel
    {
        public MaxBetChoiceViewModel() { }

        public MaxBetChoiceViewModel(string name, double maxBet, string displayName)
        {
            Name = name;
            MaxBet = maxBet;
            DisplayName = displayName;
        }

        public string Name { get; set; }

        public string DisplayName { get; }

        [Required]        
        public double MaxBet { get; set; }
    }
}
