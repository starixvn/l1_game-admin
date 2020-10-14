using GamesAdmin.Site.Features.GameUsers.Results;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.GameUsers.ViewModels
{
    public class AddViewModel
    {
        [DisplayName("Names")]
        [Required]
        public string TextNames { get; set; }

        public List<AddUsersResult> Results { get; set; }
    }
}
