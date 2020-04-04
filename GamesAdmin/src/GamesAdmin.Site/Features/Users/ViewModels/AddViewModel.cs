using GamesAdmin.Site.Features.Account.Results;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Users.ViewModels
{
    public class AddViewModel
    {
        [Required]
        [MaxLength(20)]
        [MinLength(6)]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$")]
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

        public AddUsersResult Result { get; set; }
    }
}
