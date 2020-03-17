using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Authentication.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "UserName could not be empty")]
        [MinLength(3)]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "UserName could not be empty")]
        [MinLength(4)]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}
