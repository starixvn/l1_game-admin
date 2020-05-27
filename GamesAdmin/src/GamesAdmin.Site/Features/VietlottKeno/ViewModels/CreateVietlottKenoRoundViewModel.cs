using System;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.VietlottKeno.ViewModels
{
    public class CreateVietlottKenoRoundViewModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long? RoundNo { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Today;

        public bool[] Result { get; set; } = new bool[80];
    }
}