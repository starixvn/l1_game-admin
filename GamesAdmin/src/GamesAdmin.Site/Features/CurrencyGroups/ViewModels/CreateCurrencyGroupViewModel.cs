using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.CurrencyGroups.ViewModels
{
    public class CreateCurrencyGroupViewModel : CurrencyGroupViewModel
    {
        [Required]
        public string Name { get; set; }

        public bool Enabled { get; set; } = true;

        public string PartnerId { get; set; }

        public IEnumerable<string> Currencies { get; set; }
    }
}