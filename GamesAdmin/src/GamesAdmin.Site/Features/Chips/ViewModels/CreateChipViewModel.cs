using System;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Chips.ViewModels
{
    public class CreateChipViewModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be a postive number.")]
        public int Value { get; set; }

        [Required]
        public string Color { get; set; }
    }
}