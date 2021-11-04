using System.Collections.Generic;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.ExchangeRates.ViewModels
{
    public class ExchangeRateManagementViewModel
    {
        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }
    }
}