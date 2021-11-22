using System;
using System.ComponentModel;

namespace GamesAdmin.Site.Features.ExchangeRates.ViewModels
{
    public class IndexViewModel
    {
        [DisplayName("From (GMT+7)")]
        public DateTime From { get; set; } = DateTime.UtcNow;

        [DisplayName("To (GMT+7)")]
        public DateTime To { get; set; } = DateTime.UtcNow.AddDays(1);
    }
}