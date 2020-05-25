using System;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.VietlottKeno.ViewModels
{
    public class AddVietlottKenoRoundViewModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long? RoundNo { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        public ResultItem[] Result { get; set; } = new ResultItem[20];
    }

    public class ResultItem
    {
        [Required]
        [Range(1, 80)]
        public byte Number { get; set; }
    }
}