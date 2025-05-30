using System;

namespace GamesAdmin.Core.Models.SlotGame
{
    public class JackpotPool
    {
        public string JackpotConfigId { get; set; }
        public string OperatorId { get; set; }

        public string OperatorName { get; set; }

        public string[] SiteNames { get; set; }

        public string Prize { get; set; }

        public decimal Amount { get; set; }
    }
}