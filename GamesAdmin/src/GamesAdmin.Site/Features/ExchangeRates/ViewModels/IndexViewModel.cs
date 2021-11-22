using System;

namespace GamesAdmin.Site.Features.ExchangeRates.ViewModels
{
    public class IndexViewModel
    {
        public DateTime From { get; set; } = DateTime.UtcNow;

        public DateTime To { get; set; } = DateTime.UtcNow;
    }
}