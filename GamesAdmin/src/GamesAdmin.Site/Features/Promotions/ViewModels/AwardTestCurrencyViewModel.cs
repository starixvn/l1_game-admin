using System;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class AwardTestCurrencyViewModel
    {
        public string Currency { get; set; }

        public string OperatorId { get; set; }
        
        public string PlayerId { get; set; }

        public string PlayerName { get; set; }

        public double Point { get; set; }

        public double AwardAmount { get; set; }
    }
}
