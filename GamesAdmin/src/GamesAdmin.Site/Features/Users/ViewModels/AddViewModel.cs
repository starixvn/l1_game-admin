using GamesAdmin.Site.Features._Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Users.ViewModels
{
    public class AddViewModel : ViewModelBase
    {
        [Required]
        [MaxLength(20)]
        [MinLength(4)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage="Username should only contain characters and numbers.")]
        public string Username { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(6)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }
    }
}
