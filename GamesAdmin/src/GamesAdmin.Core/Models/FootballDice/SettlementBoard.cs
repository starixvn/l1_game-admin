using System.Collections.Generic;
using Newtonsoft.Json;

namespace GamesAdmin.Core.Models.FootballDice
{
    public class FootballDiceControlResult
    {

        [JsonProperty("roundId")]
        public int RoundId { get; set; }

        [JsonProperty("isSuccessful")]
        public bool IsSuccessful { get; set; }
    }
}
