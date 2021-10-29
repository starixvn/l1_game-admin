using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Site.Features._Shared;

namespace GamesAdmin.Site.Features.Partners.ViewModels
{
    public class PartnerEditViewModel : ViewModelBase
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Partner Id should only contain characters and numbers.")]
        [DisplayName("Id")]
        public string PartnerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Secret Key")]
        public string SecretKey { get; set; }

        [Required]
        [DisplayName("Saba.club URL")]
        public string ClubVersionUrl { get; set; }


        [DisplayName("Saba.mobi URL")]
        public string MobiVersionUrl { get; set; }

        [DisplayName("Instant URL")]
        public string MiniVersionUrl { get; set; }

        [Required]
        [DisplayName("Test Currency URL")]
        public string TestCurrencyUrl { get; set; }
    }
}
