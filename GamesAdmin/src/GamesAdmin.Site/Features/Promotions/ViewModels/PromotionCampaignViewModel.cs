using System.Collections.Generic;
using System;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class PromotionCampaignViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public List<OperatorViewModel> Operators { get; set; }
        public List<WinningPrizeViewModel> WinningPrizes { get; set; } = new List<WinningPrizeViewModel>();
    }
}