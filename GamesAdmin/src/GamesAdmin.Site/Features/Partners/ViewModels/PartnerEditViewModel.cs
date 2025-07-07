using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Site.Features._Shared;

namespace GamesAdmin.Site.Features.Partners.ViewModels
{
    public class PartnerEditViewModel : ViewModelBase
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Operator Id should only contain characters, numbers, underscore and dash.")]
        [DisplayName("Id")]
        public string PartnerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Secret Key")]
        public string SecretKey { get; set; }

        [Required]
        [DisplayName("Client URL")]
        public string ClubVersionUrl { get; set; }

        [DisplayName("Mobi URL")]
        public string MobiVersionUrl { get; set; }

        [DisplayName("Instant URL")]
        public string MiniVersionUrl { get; set; }

        [DisplayName("Mega Keno URL")]
        public string MegaKenoUrl { get; set; }

        [DisplayName("Mobile Direct URL")]
        public string MobileDirectUrl { get; set; }

        [Required]
        [DisplayName("Test Currency URL")]
        public string TestCurrencyUrl { get; set; }

        [DisplayName("Livestreaming Platform URL")]
        public string LivestreamingPlatformUrl { get; set; }

        [DisplayName("Display Player Currency")]
        public bool IsDisplayPlayerCurrency { get; set; }

        [Required]
        public string Alias { get; set; }
    }
}