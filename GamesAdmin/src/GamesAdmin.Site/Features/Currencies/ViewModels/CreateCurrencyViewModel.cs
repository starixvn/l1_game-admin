using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.ViewModels
{
    public class CreateCurrencyViewModel
    {
        [Required]
        [RegularExpression("[^\\s]+", ErrorMessage = "Spaces are not permitted.")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("[^\\s]+", ErrorMessage = "Spaces are not permitted.")]
        public string Code { get; set; }

        [Required]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "The multiplier must be a positive number.")]
        public double Multiplier { get; set; }

        public bool Enabled { get; set; } = true;

        public bool IsTest { get; set; }

        [Required]
        public string PartnerId { get; set; }

        public string GroupId { get; set; }

        public IEnumerable<SelectListItem> Partners { get; set; }
    }
}