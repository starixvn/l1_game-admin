using System;

namespace GamesAdmin.Site.Features.ExchangeRates.ViewModels
{
    public class IndexViewModel
    {
        public DateTime From { get; set; } = DateTime.Now;

        public DateTime To { get; set; } = DateTime.Now;
    }
}