using System;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.ProVietKeno.ViewModels
{
    public class CreateProVietKenoRoundViewModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long? RoundNo { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Today;

        public bool[] Result { get; set; } = new bool[80];
    }
}