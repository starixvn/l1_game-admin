using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Site.Features.Bot.Results;

namespace GamesAdmin.Site.Features.Bot.ViewModels
{
    public class AddViewModel
    {
        [DisplayName("Names")]
        [Required]
        public string TextNames { get; set; }

        public List<AddBotResult> Results { get; set; }
    }
}
