using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Site.Features._Shared;

namespace GamesAdmin.Site.Features.Users.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private const int MinLength = 6;
        private const int MaxLength = 20;

        [Required]
        [MaxLength(MaxLength)]
        [MinLength(MinLength)]
        public string Password { get; set; }

        [Required]
        [MaxLength(MaxLength)]
        [MinLength(MinLength)]
        public string NewPassword { get; set; }

        [Required]
        [MaxLength(MaxLength)]
        [MinLength(MinLength)]
        [DisplayName("Confirm New Password")]
        public string ConfirmNewPassword { get; set; }
    }
}
