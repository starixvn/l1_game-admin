using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.GameSettings.ViewModels;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class PromotionViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "The Game is required field.")]
        public GameId GameId { get; set; }

        public Dictionary<string, double> Odds { get; set; }
        public IList<OddsViewModel> ListOdds { get; set; }
        public bool Enabled { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public bool IsDaily { get; set; }
        public string Status => Enabled ? PromotionStatus.Enabled.Value : PromotionStatus.Disabled.Value;
    }
}