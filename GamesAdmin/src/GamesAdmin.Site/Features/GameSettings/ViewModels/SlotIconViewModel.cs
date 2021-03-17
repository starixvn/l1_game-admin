using System;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class SlotIconViewModel
    {
        public SlotIconViewModel()
        {
        }

        public SlotIconViewModel(byte count, double odds)
        {
            Count = count;
            Odds = odds;
        }

        public byte Count { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public double Odds { get; set; }
    }
}