using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.GameUsers.ViewModels
{
    public class GetSpinViewModel
    {
        [DisplayName("Member Id")]
        [Required]
        public string MemberId { get; set; }
    }
}
