using System;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Chips.ViewModels
{
    public class CreateChipViewModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Value must greater than 0.01")]
        public double Value { get; set; }

        [Required]
        public string Color { get; set; }
    }
}