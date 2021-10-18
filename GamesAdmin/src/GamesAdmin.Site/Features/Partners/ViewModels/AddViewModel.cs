using GamesAdmin.Site.Features._Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Partners.ViewModels
{
    public class AddViewModel : ViewModelBase
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage="Partner Id should only contain characters and numbers.")]
        [DisplayName("Id")]
        public string PartnerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Secret Key")]
        public string SecretKey { get; set; }

        [Required]
        [DisplayName("Casino Version URL")]
        public string CasinoVersionUrl { get; set; }

        [Required]
        [DisplayName("Mobi Version URL")]
        public string MobiVersionUrl { get; set; }

        [Required]
        [DisplayName("Mini Version URL")]
        public string MiniVersionUrl { get; set; }

        [Required]
        [DisplayName("Api URL")]
        public string ApiUrl { get; set; }
    }
}
