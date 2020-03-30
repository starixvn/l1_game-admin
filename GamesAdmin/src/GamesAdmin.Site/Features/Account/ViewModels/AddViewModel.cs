using GamesAdmin.Site.Features.Account.Results;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Account.ViewModels
{
    public class AddViewModel
    {
        [DisplayName("Names")]
        [Required]
        public string TextNames { get; set; }

        [DisplayName("IsBot")]
        public bool IsBot { get; set; }

        public List<AddUsersResult> Results { get; set; }
    }
}
