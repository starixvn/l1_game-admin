using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class PromotionCampaignViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "The Promotion name is required field.")]
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow.AddDays(1);
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(1);
        public DateTime StartTime { get; set; } = DateTime.UtcNow.Date;
        public DateTime EndTime { get; set; } = DateTime.UtcNow.Date;
        public string Status { get; set; }
        public List<OperatorViewModel> Operators { get; set; }
        public List<WinningPrizeViewModel> WinningPrizes { get; set; } = new List<WinningPrizeViewModel>();

        public void ConvertToTimeZone(TimeZoneInfo timeZone)
        {
            StartDate = TimeZoneInfo.ConvertTimeFromUtc(StartDate, timeZone);
            EndDate = TimeZoneInfo.ConvertTimeFromUtc(EndDate, timeZone);
        }
    }
}