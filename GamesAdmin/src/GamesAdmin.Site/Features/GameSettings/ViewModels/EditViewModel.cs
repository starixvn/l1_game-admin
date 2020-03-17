using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class EditViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        [Required(ErrorMessage = "Mininum Bet could not empty")]
        [Range(0.01, 100, ErrorMessage = "Enter number between 0.01 to 100")]
        [DisplayName("Minimum Bet Limit")]
        public double MinBet { get; set; }

        [Required(ErrorMessage = "Maximum Bet could not empty")]
        [Range(0.01, 100, ErrorMessage = "Enter number between 0 to 1000")]
        [DisplayName("Maximum Bet Limit")]
        public double MaxBet { get; set; }
    }
}
