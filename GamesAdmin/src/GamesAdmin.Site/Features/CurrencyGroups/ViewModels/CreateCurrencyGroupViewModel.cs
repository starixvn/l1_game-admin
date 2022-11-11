using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.CurrencyGroups.ViewModels
{
    public class CreateCurrencyGroupViewModel : CurrencyGroupFormViewModel
    {
        [Required(ErrorMessage = "The Currency Group is required field.")]
        public string Name { get; set; }

        public bool Enabled { get; set; } = true;

        public string PartnerId { get; set; }

        public IEnumerable<string> Currencies { get; set; }

        public bool FractionalOddsFormat { get; set; }
    }
}