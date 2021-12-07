using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.ViewModels
{
    public class CreateCurrencyViewModel
    {
        [Required(ErrorMessage = "The Currency is required field.")]
        [RegularExpression("[^\\s]+", ErrorMessage = "Spaces are not permitted.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Code is required field.")]
        [RegularExpression("[^\\s]+", ErrorMessage = "Spaces are not permitted.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "The Multiplier is required field.")]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "The multiplier must be a positive number.")]
        public double Multiplier { get; set; } = 1;

        public bool Enabled { get; set; } = true;

        public bool IsTest { get; set; }

        [Required]
        public string PartnerId { get; set; }

        public string GroupId { get; set; }

        public IEnumerable<SelectListItem> Partners { get; set; }

        public IEnumerable<SelectListItem> ExchangeCodes { get; set; }
    }
}