using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class ChoiceViewModel
    {
        public ChoiceViewModel() { }

        public ChoiceViewModel(string key, double odds, string name)
        {
            Key = key;
            Odds = odds;
            Name = name;
        }

        [Required(ErrorMessage = "Key is required")]
        public string Key 
        { 
            get => _key;
            set => _key = value?.Trim();
        }

        [Required(ErrorMessage = "Name is required")]
        public string Name 
        { 
            get => _name;
            set => _name = value?.Trim();
        }

        private string _key;
        private string _name;

        private const double MIN_ODDS_VALUE = 0.01;

        [Required]
        [Range(MIN_ODDS_VALUE, int.MaxValue)]
        public double Odds { get; set; }
    }
}
