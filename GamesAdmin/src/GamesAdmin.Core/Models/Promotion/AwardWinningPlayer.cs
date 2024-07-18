using System;
using System.Collections.Generic;
using System.Text;

namespace GamesAdmin.Core.Models.Promotion
{
    public class AwardWinningPlayer
    {
        public decimal Amount { get; set; }

        public int PromotionId { get; set; }

        public string Currency { get; set; }

        public string OperatorId { get; set; }

        public string PlayerId { get; set; }

        public string SiteId { get; set; }

        public int GameTypeId { get; set; }
    }
}