using System;
using System.Collections.Generic;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.ExchangeRates.ViewModels
{
    public class ExchangeRateManagementViewModel
    {
        private const long offsetValue = 7;

        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }

        public DateTime ToGMT7(DateTime time) => time.AddTicks(TimeSpan.TicksPerHour * offsetValue);
    }
}