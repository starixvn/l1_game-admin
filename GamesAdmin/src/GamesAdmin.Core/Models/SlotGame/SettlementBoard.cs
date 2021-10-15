using System.Collections.Generic;
using Newtonsoft.Json;

namespace GamesAdmin.Core.Models.SlotGame
{
    public class SettlementBoard
    {
        public IEnumerable<byte> Result { get; set; }

        [JsonProperty("paylines")]
        public IEnumerable<PaylineItem> Paylines { get; set; }

        [JsonProperty("bonus")]
        public PaylineItem Bonus { get; set; }

        [JsonProperty("freeSpins")]
        public PaylineItem FreeSpins { get; set; }

        [JsonProperty("jackpotPayline")]
        public int JackpotPayline { get; set; }
    }

    public class PaylineItem
    {
        [JsonProperty("payline")]
        public byte Payline { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("count")]
        public byte Count { get; set; }

        [JsonProperty("odds")]
        public double Odds { get; set; }
    }
}
