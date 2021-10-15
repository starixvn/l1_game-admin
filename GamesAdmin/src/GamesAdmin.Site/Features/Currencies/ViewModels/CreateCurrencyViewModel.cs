using System;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Currencies.ViewModels
{
    public class CreateCurrencyViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Rate { get; set; }

        public bool Enabled { get; set; } = true;

        public bool IsTest { get; set; }

        [Required]
        public string PartnerId { get; set; }

        public string GroupId { get; set; }

        public string Message { get; set; }

        public bool Success { get; set; }
    }
}