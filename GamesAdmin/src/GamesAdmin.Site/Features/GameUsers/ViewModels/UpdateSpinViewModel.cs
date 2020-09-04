using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.GameUsers.ViewModels
{
    public class UpdateSpinViewModel
    {
        public UpdateSpinViewModel(int memberId)
        {
            MemberId = memberId;
        }

        public int MemberId { get; set; }

        [DisplayName("Avail Spins")]
        [Required]
        public string AvailSpins { get; set; }

        [DisplayName("Expiry Date")]
        [Required]
        public DateTimeOffset ExpiryDate { get; set; }
    }
}
